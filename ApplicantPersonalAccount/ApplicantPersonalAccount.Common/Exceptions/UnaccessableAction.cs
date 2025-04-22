using Microsoft.AspNetCore.Http;

namespace ApplicantPersonalAccount.Common.Exceptions
{
    public class UnaccessableAction : CustomException
    {
        public UnaccessableAction(string message) :
           base(StatusCodes.Status403Forbidden, "Forbid", message)
        { }
    }
}
