using System.Security.Claims;

namespace ApplicantPersonalAccount.Infrastructure.Utilities
{
    public class UserDescriptor
    {
        public static Guid GetUserId(ClaimsPrincipal principal)
        {
            string? userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new UnauthorizedAccessException();

            return new Guid(userId);
        }

        public static string GetUserEmail(ClaimsPrincipal principal)
        {
            string? userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail == null)
                throw new UnauthorizedAccessException();

            return userEmail;
        }
    }
}
