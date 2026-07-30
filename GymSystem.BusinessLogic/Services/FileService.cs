using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymSystem.BusinessLogic.Services;

public class FileService(IWebHostEnvironment environment) : IFileService
{
    private readonly IWebHostEnvironment _environment = environment;

    public async Task<Result<string>> UploadAsync(
        IFormFile file, string folderName, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            return Result.Fail<string>("Please select a file to upload.");

        // 1. Check the extension.
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!FileSettings.AllowedExtensions.Contains(extension))
        {
            return Result.Fail<string>(
                $"Only {FileSettings.AllowedExtensionsDisplay} files are allowed.");
        }

        // 2. Check the size.
        if (file.Length > FileSettings.MaxFileSizeInBytes)
        {
            return Result.Fail<string>(
                $"File size must not exceed {FileSettings.MaxFileSizeDisplay}.");
        }

        // 3. Locate the folder, creating it if missing.
        var folderPath = BuildFolderPath(folderName);
        Directory.CreateDirectory(folderPath);

        // 4. Make the name unique so two "photo.png" uploads never collide.
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";

        // 5. Build the full path.
        var filePath = Path.Combine(folderPath, uniqueFileName);

        // 6/7. Copy into the stream. 'await using' closes it even if the copy throws.
        await using (var stream = File.Create(filePath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // 8. Return the name to store on the entity.
        return uniqueFileName;
    }

    public Result DeleteFile(string folderName, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Fail("No file name was supplied.");

        var filePath = BuildFilePath(folderName, fileName);

        if (filePath is null || !File.Exists(filePath))
            return Result.NotFound("The file was not found.");

        File.Delete(filePath);

        return Result.Ok();
    }

    public Result<StoredFile> GetFile(string folderName, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Fail<StoredFile>("No file name was supplied.");

        var filePath = BuildFilePath(folderName, fileName);

        if (filePath is null || !File.Exists(filePath))
            return Result.NotFound<StoredFile>("The file was not found.");

        var content = File.OpenRead(filePath);

        return new StoredFile(content, ResolveContentType(filePath), fileName);
    }

    private string BuildFolderPath(string folderName)
        => Path.Combine(_environment.ContentRootPath, FileSettings.UploadsRootFolder, folderName);

    /// <summary>
    /// Combines folder and file name, rejecting anything that escapes the uploads root
    /// (e.g. a stored name containing "../").
    /// </summary>
    private string? BuildFilePath(string folderName, string fileName)
    {
        var folderPath = BuildFolderPath(folderName);
        var fullPath = Path.GetFullPath(Path.Combine(folderPath, fileName));

        var rootPath = Path.GetFullPath(folderPath);

        return fullPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase)
            ? fullPath
            : null;
    }

    private static string ResolveContentType(string filePath)
        => Path.GetExtension(filePath).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            _ => "image/jpeg"
        };
}
