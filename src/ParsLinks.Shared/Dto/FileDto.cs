using Microsoft.AspNetCore.Components.Forms;

namespace ParsLinks.Shared.Dto;

public class FileDto(IBrowserFile file)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public byte[] Bytes { get; set; } = null;
    public StreamContent? StreamContent { get; set; }
    public List<byte[]> ChunkedBytes { get; set; } = new();
    public long ChunkSize { get; set; }
    public long Size { get; set; } = file.Size;
    public long UploadedBytes { get; set; }
    public double UploadedPercentage => (double)UploadedBytes / file.Size * 100d;
    public bool IsUploaded { get; set; } = false;
    public string FileName { get; set; } = Guid.NewGuid().ToString() + Path.GetExtension(file.Name);
    public string Name { get; set; } = file.Name;
    public string ContentType { get; set; } = file.ContentType;
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    public DateTimeOffset LastModifed { get; set; } = file.LastModified;
    public string? ErrorMessage { get; set; }
    public string Checksum { get; set; } = default!;
    public string? Base64 { get; set; }
}
