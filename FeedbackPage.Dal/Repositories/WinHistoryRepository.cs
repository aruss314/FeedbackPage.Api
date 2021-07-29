using Dapper;
using FeedbackPage.Dal.Models;
using System;
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

        public async Task<int> CreateWinHistoryRecord(WinHistoryRecord record)
        {
            var query = $@"INSERT INTO {targetTable}(player_name,player_score,time_of_victory_utc)
                VALUES(@PlayerName,@PlayerScore,@TimeOfVictoryUtc) RETURNING id";
            if (connection.State != ConnectionState.Open) { connection.Open(); }

            var results = await connection.ExecuteScalarAsync<int>(query, new
            {
                record.PlayerName,
                record.PlayerScore,
                record.TimeOfVictoryUtc
            });
            return results;
        }

        public async Task<int> DeleteWinHistoryRecord(int id)
        {
            var query = $@"DELETE FROM {targetTable} WHERE id = @Id";
            if (connection.State != ConnectionState.Open) { connection.Open(); }

            return await connection.ExecuteAsync(query, new
            {
                Id = id
            });
        }

        public async Task<WinHistoryRecord> GetWinHistoryRecord(int id)
        {
            var query = $@"SELECT id AS Id, player_name AS PlayerName, player_score AS PlayerScore, time_of_victory_utc AS TimeOfVictoryUtc FROM {targetTable} WHERE id = @Id";
            if (connection.State != ConnectionState.Open) { connection.Open(); }

            var response = await connection.QueryFirstOrDefaultAsync<WinHistoryRecord>(query, new
            {
                Id = id
            });
            return response;
        }

        public async Task<IEnumerable<WinHistoryRecord>> GetWinHistoryRecords(WinHistoryRecordRequest request)
        {
            var sQuery = $"SELECT id AS Id, player_name AS PlayerName, player_score AS PlayerScore, time_of_victory_utc AS TimeOfVictoryUtc FROM {targetTable} WHERE player_name = @PlayerName ORDER BY time_of_victory_utc DESC LIMIT @PageSize OFFSET @Offset";
            if (connection.State != ConnectionState.Open) { connection.Open(); }
           
            return await connection.QueryAsync<WinHistoryRecord>(sQuery, new {request.PlayerName, request.PageSize, Offset = request.PageNumber * request.PageSize });
        }



    }
}
