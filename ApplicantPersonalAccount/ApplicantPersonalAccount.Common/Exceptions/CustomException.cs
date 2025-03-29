namespace ApplicantPersonalAccount.Common.Exceptions
{
    public class CustomException : Exception
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }

        public CustomException(int code, string error, string message)
            : base(message)
        {
            Code = code;
            Error = error;
            Message = message;
        }
    }
}
