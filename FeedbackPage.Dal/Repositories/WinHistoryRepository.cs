using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FeedbackPage.Dal.Repositories
{
    public class WinHistoryRepository : IWinHistoryRepository
    {
        private readonly IDbConnection connection;
        private const string targetTable = "win_history";
        public WinHistoryRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> CreateWinHistoryRecord(object record)
        {
           // throw new System.NotImplementedException();
            if( connection.State != ConnectionState.Open) { connection.Open(); }

            var query = $"Query that creates and record and returns the new record id";

            var results = await connection.ExecuteScalarAsync<int>(query, new
            {
                id = "parameters here"
            });
            return results;
        }

        public async Task DeleteWinHistoryRecord(int id)
        {
            throw new System.NotImplementedException();
            if (connection.State != ConnectionState.Open) { connection.Open(); }

            var query = $"Query that deletes a record by it's id";

            var results = await connection.ExecuteAsync(query, new
            {
                id = "parameters here"
            });
        }

        public async Task<object> GetWinHistoryRecord(int id)
        {
            throw new System.NotImplementedException();
            if (connection.State != ConnectionState.Open) { connection.Open(); }

            var query = $"Query gets a record by id";

            var results = await connection.QueryAsync<int>(query, new
            {
                id = "parameters here"
            });
            return results;
        }

        public async Task<IEnumerable<object>> GetWinHistoryRecord(IEnumerable<int> ids)
        {
            throw new System.NotImplementedException();
            if (connection.State != ConnectionState.Open) { connection.Open(); }

            var query = $"Query that gets a list of records by ids";

            var results = await connection.QueryAsync<object>(query, new
            {
                id = "parameters here"
            });
            return results;
        }
    }
}
