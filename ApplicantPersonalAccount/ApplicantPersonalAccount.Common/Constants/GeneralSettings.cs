namespace ApplicantPersonalAccount.Common.Constants
{
    public class GeneralSettings
    {
        public const int ACCESS_TOKEN_LIFETIME = 10; // in minutes
        public const int REFRESH_TOKEN_LIFETIME = 24; // in hours
        public static readonly List<string> ALLOWED_FILE_EXTENSIONS = new() { ".pdf", ".png", ".jpg" };
        public const int MAX_FILE_SIZE = 30 * 1024 * 1024; // in MB
    }
}
