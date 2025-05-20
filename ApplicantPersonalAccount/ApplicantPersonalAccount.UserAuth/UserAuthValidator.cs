using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Models.Authorization;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.UserAuth
{
    public class UserAuthValidator
    {
        public static List<string?> ValidateUserRegister(UserRegisterModel model)
        {
            var errors = new List<string?>();

            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, context, results, true))
                errors.AddRange(results.Select(r => r.ErrorMessage));

            if (model.Birthdate > DateTime.Now.ToUniversalTime())
                errors.Add(ValidationErrors.BIRTHDAY_IN_FUTURE);

            return errors;
        }
    }
}
