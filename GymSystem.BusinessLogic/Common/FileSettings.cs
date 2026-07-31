namespace GymSystem.BusinessLogic.Common;

public static class FileSettings
{

    public const string UploadsRootFolder = "Uploads";

    public const string MemberPhotosFolder = "MemberPhotos";

    public const long MaxFileSizeInBytes = 5 * 1024 * 1024;
    public const string MaxFileSizeDisplay = "5 MB";

    public static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];

    public static string AllowedExtensionsDisplay => string.Join(", ", AllowedExtensions);
}
