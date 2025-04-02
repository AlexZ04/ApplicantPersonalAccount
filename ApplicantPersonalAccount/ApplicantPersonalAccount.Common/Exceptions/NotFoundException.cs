using Microsoft.AspNetCore.Http;

namespace ApplicantPersonalAccount.Common.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : 
            base(StatusCodes.Status404NotFound, "Not found", message) 
        { }
    }
}
