using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Models.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.API.Validator
{
    public static class Validator
    {
        public static List<string?> ValidateUserRegister(UserRegisterModel model)
        {
            var errors = new List<string?>();

            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();

            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, context, results, true))
                errors.AddRange(results.Select(r => r.ErrorMessage));

            if (model.Birthdate > DateTime.Now.ToUniversalTime())
                errors.Add(ValidationErrors.BIRTHDAY_IN_FUTURE);

            return errors;
        }

        public static List<string?> ValidatePagination(int page, int size)
        {
            var errors = new List<string?>();
            var results = new List<ValidationResult>();

            if (page <= 0 || size <= 0)
                errors.Add(ValidationErrors.INVALID_PAGINATION);

            return errors;
        }

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
