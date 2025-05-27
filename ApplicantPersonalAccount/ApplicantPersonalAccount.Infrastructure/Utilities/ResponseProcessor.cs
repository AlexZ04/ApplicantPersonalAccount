using ApplicantPersonalAccount.Common.Exceptions;

namespace ApplicantPersonalAccount.Infrastructure.Utilities
{
    public class ResponseProcessor
    {
        public static void ProcessResponse(string response)
        {
            if (response == null)
                throw new ProcessingException();
        }
    }
}
