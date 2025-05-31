using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Staff.Domain.Infrascructure
{
    public class TokenAuthFilterAttribute : TypeFilterAttribute
    {
        public TokenAuthFilterAttribute() : base(typeof(TokenAuthFilter)) { }
    }
}
