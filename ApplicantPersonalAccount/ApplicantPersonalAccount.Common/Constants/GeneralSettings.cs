namespace ApplicantPersonalAccount.Common.Constants
{
    public class GeneralSettings
    {
        public const int ACCESS_TOKEN_LIFETIME = 10 * 3600; // in minutes
        public const int REFRESH_TOKEN_LIFETIME = 24; // in hours
        public static readonly List<string> ALLOWED_FILE_EXTENSIONS = new() { ".pdf", ".png", ".jpg" };
        public const int MAX_FILE_SIZE = 30 * 1024 * 1024; // in MB
        public const int MAX_CHOSEN_PROGRAMS = 5;
        public const int RPC_TIMEOUT = 15; // in seconds
    }
}
