using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Security.Cryptography;

namespace ParsLinks.Shared.Extensions;

public static class FileSizeExtensions
{
    private static readonly string[] SizeSuffixes = { "byte", "KB", "MB", "GB", "TB", "PB", "EB" };

    public static string ByteToString(this long byteCount)
    {
        if (byteCount == null)
            return string.Empty;

        if (byteCount == 0)
            return $"0 {SizeSuffixes[0]}";

        int place = (int)Math.Floor(Math.Log(byteCount, 1024));
        double size = Math.Round(byteCount / Math.Pow(1024, place), 1);
        return $"{size} {SizeSuffixes[place]}";
    }
}

public static class TimeExtensions
{
    public static string ToElapsedTime(this DateTime? date)
    {
        if (date == null)
            return string.Empty;

        TimeSpan timeSince = DateTime.UtcNow - date.GetValueOrDefault();

        if (timeSince.TotalSeconds < 5)
            return "just now";
        else if (timeSince.TotalSeconds < 60)
            return $"{Math.Floor(timeSince.TotalSeconds)} seconds ago";
        else if (timeSince.TotalMinutes < 60)
            return $"{Math.Floor(timeSince.TotalMinutes)} minutes ago";
        else if (timeSince.TotalHours < 24)
            return $"{Math.Floor(timeSince.TotalHours)} hours ago";
        else if (timeSince.TotalDays < 7)
            return $"{Math.Floor(timeSince.TotalDays)} days ago";
        else
            return date.GetValueOrDefault().ToString("MMMM dd, yyyy");
    }

    public static string ToElapsedTime(this DateTime date)
    {
        if (date == null)
            return string.Empty;

        TimeSpan timeSince = DateTime.UtcNow - date;

        if (timeSince.TotalSeconds < 5)
            return "just now";
        else if (timeSince.TotalSeconds < 60)
            return $"{Math.Floor(timeSince.TotalSeconds)} seconds ago";
        else if (timeSince.TotalMinutes < 60)
            return $"{Math.Floor(timeSince.TotalMinutes)} minutes ago";
        else if (timeSince.TotalHours < 24)
            return $"{Math.Floor(timeSince.TotalHours)} hours ago";
        else if (timeSince.TotalDays < 7)
            return $"{Math.Floor(timeSince.TotalDays)} days ago";
        else
            return date.ToString("MMMM dd, yyyy");
    }
}

public static class StreamExtensions
{
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
    public static string CalculateSha256Checksum(this Stream stream)
    {
        using (var hasher = SHA256.Create())
        {
            const int bufferSize = 8192; // 8 KB buffer size
            byte[] buffer = new byte[bufferSize];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, bufferSize)) > 0)
            {
                hasher.TransformBlock(buffer, 0, bytesRead, buffer, 0);
            }

            hasher.TransformFinalBlock(buffer, 0, 0);
            return BitConverter.ToString(hasher.Hash).Replace("-", "").ToLowerInvariant();
        }
    }
    public static string CalculateSha256Checksum(this byte[] data)
    {
        using (var hasher = SHA256.Create())
        {
            byte[] hashBytes = hasher.ComputeHash(data);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
    private static string CalculateSha256Checksum(string filePath)
    {
        using (var hasher = SHA256.Create())
        using (var stream = File.OpenRead(filePath))
        {
            const int bufferSize = 8192; // 8 KB buffer size
            byte[] buffer = new byte[bufferSize];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, bufferSize)) > 0)
            {
                hasher.TransformBlock(buffer, 0, bytesRead, buffer, 0);
            }

            hasher.TransformFinalBlock(buffer, 0, 0);
            return BitConverter.ToString(hasher.Hash).Replace("-", "").ToLowerInvariant();
        }
    }

}



public static class FileExtensions
{
    public static async Task<byte[]> ToBytesAsync(this IBrowserFile file, CancellationToken cancellationToken = default!)
    {

        // Open a stream to read the file content with the specified maxFileSize limit
        using (var data = file.OpenReadStream(file.Size))
        {
            // Create a MemoryStream to store the file content
            using (var memoryStream = new MemoryStream())
            {
                // Copy the file content to the MemoryStream asynchronously
                await data.CopyToAsync(memoryStream, cancellationToken);

                // Return the byte array from the MemoryStream
                return memoryStream.ToArray();
            }
        }



    }
    public static List<byte[]> Chunk(this byte[] source, long chunkSize)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (chunkSize <= 0)
            throw new ArgumentException("Chunk size must be greater than 0.", nameof(chunkSize));

        List<byte[]> chunks = new List<byte[]>();
        int length = source.Length;

        for (long i = 0; i < length; i += chunkSize)
        {
            long currentChunkSize = Math.Min(chunkSize, length - i);
            byte[] chunk = new byte[currentChunkSize];
            Array.Copy(source, i, chunk, 0, currentChunkSize);
            chunks.Add(chunk);
        }

        return chunks;
    }

    public static async Task<List<byte[]>> GetChunkedBytesAsync(this IBrowserFile file, long chunkSize, CancellationToken cancellationToken = default)
    {
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        var chunkedBytes = new List<byte[]>();

        var bytes = await file.ToBytesAsync(cancellationToken);

        for (long i = 0; i < bytes.Length; i += chunkSize)
        {
            var chunkLength = Math.Min(chunkSize, bytes.Length - i);
            var chunk = new byte[chunkLength];
            Array.Copy(bytes, i, chunk, 0, chunkLength);
            chunkedBytes.Add(chunk);
        }

        return chunkedBytes;
    }

    public static async Task<byte[]> ResizeImageAsync(this Stream stream, int width, int height)
    {
        using var image = await Image.LoadAsync(stream);
        image.Mutate(x => x.Resize(width, height));
        using var memoryStream = new MemoryStream();
        await image.SaveAsJpegAsync(memoryStream);
        return await memoryStream.ToByteArrayAsync();

    }
    public static async Task<byte[]> ResizeImageAsync(this byte[] bytes, int width, int height)
    {
        using var memoryStream = new MemoryStream(bytes);
        using var image = await Image.LoadAsync(memoryStream);
        image.Mutate(x => x.Resize(width, height));
        using var outputStream = new MemoryStream();
        await image.SaveAsJpegAsync(outputStream);
        return await outputStream.ToByteArrayAsync();
    }

    public static byte[] ResizeImage(this byte[] bytes, int width, int height)
    {
        using var memoryStream = new MemoryStream(bytes);
        using var image = Image.Load(memoryStream);
        var resizeOprtion = new ResizeOptions
        {
            Size = new Size(width, height),
        };
        image.Mutate(x => x.Resize(resizeOprtion));
        using var outputStream = new MemoryStream();
        image.SaveAsJpeg(outputStream);
        return outputStream.ToArray();
    }

    public static async Task<string> ToBase64StringAsync(this IBrowserFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.OpenReadStream().CopyToAsync(memoryStream);
            return Convert.ToBase64String(memoryStream.ToArray());
        }
    }


}


public static class StringExtensions
{
    public static List<string> ToListOfStrings(this string? commaSeparatedString)
    {
        if (string.IsNullOrWhiteSpace(commaSeparatedString))
        {
            return new List<string>();
        }

        return commaSeparatedString.Split(',').ToList();
    }
}
