using System;

namespace FeedbackPage.Dal.Models
{
    public class WinHistoryRecord
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }
        public DateTime TimeOfVictoryUtc { get; set; }
    }
}
