using ApplicantPersonalAccount.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Document
{
    public class FileControllerValidator
    {
        public static List<string?> ValidateFile(IFormFile file)
        {
            var errors = new List<string?>();
            var results = new List<ValidationResult>();

            if (file == null || file.Length == 0)
                errors.Add(ErrorMessages.INVALID_FILE);

            var extension = Path.GetExtension(file?.FileName)?.ToLowerInvariant();
            if (extension == null || extension.Length < 2 ||
                !GeneralSettings.ALLOWED_FILE_EXTENSIONS.Contains(extension))
                errors.Add(ErrorMessages.INVALID_FILE_EXTENSION);

            if (file?.Length > GeneralSettings.MAX_FILE_SIZE)
                errors.Add(ErrorMessages.FILE_IS_TOO_BIG);

            return errors;
        }
    }
}
