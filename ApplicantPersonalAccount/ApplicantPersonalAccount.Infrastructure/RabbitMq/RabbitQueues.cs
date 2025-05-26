namespace ApplicantPersonalAccount.Infrastructure.RabbitMq
{
    public class RabbitQueues
    {
        public const string NOTIFICATION = "notification_queue";
        public const string SUBS = "subscribtions_queue";

        public const string GET_DOCUMENT_TYPE = "document_type_queue";
        public const string GET_DIRECTORY_PROGRAMS = "directory_programs_queue";
    }
}
