namespace ApplicantPersonalAccount.Common.Models.Enterance
{
    public class EnterancePagedListModel
    {
        public List<EnteranceModel> Enterances { get; set; } = new List<EnteranceModel>();
        public PageInfoModel Pagination { get; set; } = new PageInfoModel();
    }
}
