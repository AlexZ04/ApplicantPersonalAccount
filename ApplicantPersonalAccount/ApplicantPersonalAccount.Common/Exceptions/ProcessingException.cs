using ApplicantPersonalAccount.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace ApplicantPersonalAccount.Common.Exceptions
{
    public class ProcessingException : CustomException
    {
        public ProcessingException() : base(
            StatusCodes.Status202Accepted, 
            "Processing",
            ErrorMessages.ONE_OF_THE_SERVICES_NOT_WORKING)
        {
        }
    }
}
