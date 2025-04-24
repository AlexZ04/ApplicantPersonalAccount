using Microsoft.AspNetCore.Http;

namespace ApplicantPersonalAccount.Common.Exceptions
{
    public class EarlyActionException : CustomException
    {
        public EarlyActionException(string message)
            : base(StatusCodes.Status400BadRequest, "Early action", message) { }
    }
}
