namespace ApplicantPersonalAccount.Infrastructure.RabbitMq
{
    public class RabbitQueues
    {
        public const string NOTIFICATION = "notification_queue";
        public const string SUBS = "subscribtions_queue";

        public const string GET_DOCUMENT_TYPE = "document_type_queue";
        public const string GET_DIRECTORY_PROGRAMS = "directory_programs_queue";
        public const string GET_EDUCATION_PROGRAM_BY_ID = "education_program_by_id";

        public const string GET_USER_BY_ID = "get_user_queue";
        public const string GET_INFO_FOR_EVENTS = "get_info_events_queue";
        public const string EDIT_INFO_FOR_EVENTS = "edit_info_events_queue";

        public const string GET_USER_DOCUMENTS = "user_documents_queue";

        public const string CAN_EDIT_LISTENER = "can_edit_queue";
    }
}
