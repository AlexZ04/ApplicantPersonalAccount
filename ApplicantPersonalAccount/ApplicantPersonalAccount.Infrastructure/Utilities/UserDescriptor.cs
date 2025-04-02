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
    }
}
