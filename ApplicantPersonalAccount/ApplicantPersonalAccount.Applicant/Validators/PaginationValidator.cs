using ApplicantPersonalAccount.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Applicant.Validators
{
    public class PaginationValidator
    {
        public static List<string?> ValidatePagination(int page, int size)
        {
            var errors = new List<string?>();
            var results = new List<ValidationResult>();

            if (page <= 0 || size <= 0)
                errors.Add(ValidationErrors.INVALID_PAGINATION);

            return errors;
        }
    }
}
