using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FeedbackPage.Api.Models;
using System;
using FeedbackPage.Api.Extensions;

namespace FeedbackPage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntervalController : ControllerBase
    {
        /// <summary>
        /// This endpoint takes two intervals and checks for overlap.
        /// </summary>
        /// <param name="intervalSubmission"></param>
        /// <returns>
        ///   if intervals operlap, return true
        ///   else, return false
        /// </returns>
        [HttpPost]
        public IActionResult CheckOverlap(IntervalsSubmission intervalSubmission)
        {
            try
            {
                return Ok(new GenericDataResponse<bool>
                {
                    Success = true,
                    Data = intervalSubmission.CheckOverlap()
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
    }
}