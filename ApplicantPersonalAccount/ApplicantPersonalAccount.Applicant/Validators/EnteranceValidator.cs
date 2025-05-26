using ApplicantPersonalAccount.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Applicant.Validators
{
    public class EnteranceValidator
    {
        public static List<string?> ValidateEnterancePriority(int priority)
        {
            var errors = new List<string?>();
            var results = new List<ValidationResult>();

            if (priority <= 0)
                errors.Add(ErrorMessages.INVALID_PRIORITY);

            return errors;
        }
    }
}
