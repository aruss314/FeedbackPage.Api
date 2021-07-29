using FeedbackPage.Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackPage.Dal.Repositories
{
    public interface IWinHistoryRepository
    {
        Task<int> CreateWinHistoryRecord(WinHistoryRecord record);
        Task<WinHistoryRecord> GetWinHistoryRecord(int id);
        Task<IEnumerable<WinHistoryRecord>> GetWinHistoryRecords(WinHistoryRecordRequest request);
        Task<int> DeleteWinHistoryRecord(int id);

    }
}
