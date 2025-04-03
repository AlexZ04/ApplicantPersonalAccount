using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Common.Models.User
{
    public class EmailEditModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
