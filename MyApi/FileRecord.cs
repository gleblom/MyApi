namespace MyApi
{
    public class FileRecord
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }
        public string StorageKey { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
        public string? UploadedBy { get; set; }
    }

}
