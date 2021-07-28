using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackPage.Dal.Repositories
{
    public interface IWinHistoryRepository
    {
        Task<int> CreateWinHistoryRecord(object record);
        Task<object> GetWinHistoryRecord(int id);
        Task<IEnumerable<object>> GetWinHistoryRecord(IEnumerable<int> ids);
        Task DeleteWinHistoryRecord(int id);

    }
}
