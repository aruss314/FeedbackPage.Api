using FeedbackPage.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTest.FeedbackPage.Api.Mocks
{
    public class NoOpWinHistoryRepository : IWinHistoryRepository
    {
        public async Task<int> CreateWinHistoryRecord(object record)
        {
            return new Random().Next();
        }

        public async Task DeleteWinHistoryRecord(int id)
        {
            return;
        }

        public async Task<object> GetWinHistoryRecord(int id)
        {
            return new object();
        }

        public async Task<IEnumerable<object>> GetWinHistoryRecord(IEnumerable<int> ids)
        {
            return new List<object>();
        }
    }
}
