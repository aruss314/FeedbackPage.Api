using FeedbackPage.Dal.Models;
using FeedbackPage.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTest.FeedbackPage.Api.Mocks
{
    public class NoOpWinHistoryRepository : IWinHistoryRepository
    {
        public async Task<int> CreateWinHistoryRecord(WinHistoryRecord record)
        {
            return new Random().Next();
        }

        public async Task<int> DeleteWinHistoryRecord(int id)
        {
            return 1;
        }

        public async Task<WinHistoryRecord> GetWinHistoryRecord(int id)
        {
            return new WinHistoryRecord { Id = id };
        }

        public async Task<IEnumerable<WinHistoryRecord>> GetWinHistoryRecords(WinHistoryRecordRequest request)
        {
            return new List<WinHistoryRecord>();
        }
    }
}
