using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using MyApi;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMinioClient _minio;
    private readonly string _bucket;

    public FilesController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _bucket = config["MinIO:Bucket"];

        _minio = new MinioClient()
            .WithEndpoint(config["MinIO:Endpoint"])
            .WithCredentials(config["MinIO:AccessKey"], config["MinIO:SecretKey"])
            .WithSSL()
            .Build();
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не выбран");

        var storageKey = $"{Guid.NewGuid()}_{file.FileName}";

        // Загружаем файл в MinIO
        using var stream = file.OpenReadStream();
        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucket)
            .WithObject(storageKey)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType));

        // Сохраняем метаданные в БД
        var record = new FileRecord
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Length,
            StorageKey = storageKey,
            UploadedAt = DateTime.UtcNow
        };

        _db.Files.Add(record);
        await _db.SaveChangesAsync();

        return Ok(record);
    }
}

