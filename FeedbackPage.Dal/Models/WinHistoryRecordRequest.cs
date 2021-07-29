namespace FeedbackPage.Dal.Models
{
    public class WinHistoryRecordRequest
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string PlayerName { get; set; }
    }
}
