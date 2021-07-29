using FeedbackPage.Api.Models;
using FeedbackPage.Dal.Models;
using FeedbackPage.Dal.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackPage.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WinHistoryController : ControllerBase 
    {
        private readonly IWinHistoryRepository repo;

        public WinHistoryController(IWinHistoryRepository repo)
        {
            this.repo = repo;
        }
        [HttpPost]
        public async Task<IActionResult> CreateWinHistoryRecord(WinHistoryRecordSubmission request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.PlayerName)) return BadRequest(new GenericResponse { Errors = "invalid player name, cannot be null, empty, or only whitespace." });
                if (request.PlayerScore < 1) return BadRequest(new GenericResponse { Errors = "player score must be greater than 0" });


                var record = new WinHistoryRecord {
                    PlayerName = request.PlayerName,
                    PlayerScore = request.PlayerScore,
                    TimeOfVictoryUtc = DateTime.UtcNow
                };
                var response = await repo.CreateWinHistoryRecord(record);
                return Ok(new GenericDataResponse<int>
                {
                    Success = true,
                    Data = response
                });
            }
            catch( Exception e)
            {
                return Ok(new GenericResponse
                {
                    Errors = e.Message
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetWinHistoryRecordById(int id)
        {
            try
            {
                if (id < 1) return BadRequest(new GenericResponse { Errors = "id cannot be less than 1." });

                WinHistoryRecord response = await repo.GetWinHistoryRecord(id);
                return Ok(new GenericDataResponse<WinHistoryRecord>
                {
                    Success = true,
                    Data = response
                });
            }
            catch (Exception e)
            {
                return Ok(new GenericResponse
                {
                    Errors = e.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetPageOfWinHistoryRecordsByPlayerName(WinHistoryRecordRequest request) {

            try {
                var response = await repo.GetWinHistoryRecords(request);
                return Ok(new GenericDataResponse<IEnumerable<WinHistoryRecord>>
                {
                    Success = true,
                    Data = response
                });
               }

            catch (Exception e)
            {
                return Ok(new GenericResponse
                {
                    Errors = e.Message
                });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteWinHistoryRecordById(int id) {
            try
            {
                var response = await repo.DeleteWinHistoryRecord(id);
                return Ok(new GenericDataResponse<int> { Success = true, Data = response });
            }
            catch (Exception e)
            {
                return Ok(new GenericResponse
                {
                    Errors = e.Message
                });
            }
        }
    }
}
