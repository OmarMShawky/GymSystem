using Microsoft.AspNetCore.Http;

namespace GymSystem.BusinessLogic.Services;

public sealed record StoredFile(Stream Content, string ContentType, string FileName);

public interface IFileService
{

    Task<Result<string>> UploadAsync(IFormFile file, string folderName, CancellationToken cancellationToken = default);

    Result DeleteFile(string folderName, string fileName);

    Result<StoredFile> GetFile(string folderName, string fileName);
}
