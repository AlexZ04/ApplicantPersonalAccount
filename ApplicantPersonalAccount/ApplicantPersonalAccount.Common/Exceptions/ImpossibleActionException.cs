using Microsoft.AspNetCore.Http;

namespace ApplicantPersonalAccount.Common.Exceptions
{
    public class ImpossibleActionException : CustomException
    {
        public ImpossibleActionException(string message) 
            : base(StatusCodes.Status400BadRequest, "Impossible action", message) { }
    }
}
