namespace GymSystem.BusinessLogic.Common;

/// <summary>
/// Upload rules and folder names in one place, so every module stores files the same way.
/// </summary>
public static class FileSettings
{
    /// <summary>Root folder, kept OUTSIDE wwwroot so files are not publicly reachable.</summary>
    public const string UploadsRootFolder = "Uploads";

    public const string MemberPhotosFolder = "MemberPhotos";

    public const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
    public const string MaxFileSizeDisplay = "5 MB";

    public static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];

    public static string AllowedExtensionsDisplay => string.Join(", ", AllowedExtensions);
}
