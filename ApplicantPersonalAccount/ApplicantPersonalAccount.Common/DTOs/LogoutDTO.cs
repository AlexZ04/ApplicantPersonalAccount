namespace ApplicantPersonalAccount.Common.DTOs
{
    public class LogoutDTO
    {
        public Guid UserId { get; set; }
        public string? Token { get; set; }
    }
}
