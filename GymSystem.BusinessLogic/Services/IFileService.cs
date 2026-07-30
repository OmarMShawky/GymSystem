using Microsoft.AspNetCore.Http;

namespace GymSystem.BusinessLogic.Services;

/// <summary>A stored file opened for reading, ready to be streamed to the browser.</summary>
public sealed record StoredFile(Stream Content, string ContentType, string FileName);

public interface IFileService
{
    /// <summary>
    /// Validates the file, saves it under a unique GUID name and returns that name
    /// for storing on the entity.
    /// </summary>
    Task<Result<string>> UploadAsync(IFormFile file, string folderName, CancellationToken cancellationToken = default);

    /// <summary>Removes a stored file. Fails if it is not there.</summary>
    Result DeleteFile(string folderName, string fileName);

    /// <summary>Opens a stored file for reading so an action can stream it back.</summary>
    Result<StoredFile> GetFile(string folderName, string fileName);
}
