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
        
        public const string GET_DOCUMENT_TYPE_BY_ID = "document_type_id_queue";
        public const string IMPORT_STATUS = "import_status_queue";
        public const string IMPORT_REQUEST = "import_request_queue";

        public const string GET_USER_DOCUMENTS = "user_documents_queue";

        public const string CAN_EDIT_LISTENER = "can_edit_queue";

        public const string LOGIN = "login_queue";
        public const string REFRESH_LOGIN = "refresh_token_login";
        public const string LOGOUT = "logout_queue";

        public const string GET_ALL_MANAGERS = "get_managers_queue";
        public const string DELETE_MANAGER = "delete_manager_queue";
        public const string UPDATE_MANAGER = "update_manager_queue";
        public const string CREATE_MANAGER = "create_manager_queue";

        public const string GET_FILTERED_NAMES = "filtered_name_queue";
        public const string GET_FILTERED_PROGRAMS = "filtered_program_queue";

        public const string CREATE_ENTERANCE = "create_enterance_queue";

        public const string GET_EDUCATION_INFO = "education_info_queue";
    }
}
