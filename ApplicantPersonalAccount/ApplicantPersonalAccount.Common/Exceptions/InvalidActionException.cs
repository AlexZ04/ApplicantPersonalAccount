using Microsoft.AspNetCore.Http;

namespace ApplicantPersonalAccount.Common.Exceptions
{
    public class InvalidActionException : CustomException
    {
        public InvalidActionException(string message)
            : base(StatusCodes.Status400BadRequest, "Invalid action", message) { }
    }
}
