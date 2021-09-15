using FeedbackPage.Api.Models;

namespace FeedbackPage.Api.Extensions
{
    public static class IntervalSubmissionsExtensions
    {
        public static bool CheckOverlap(this IntervalsSubmission intervals) 
        {
            //if ( intervals.B1 > intervals.A2 ) { return false; }
            //if ( intervals.B2 < intervals.A1 ) { return false; }
            //return true;
            // if ( (intervals.B1 > intervals.A2) || (intervals.B2 < intervals.A1)) { return false; }
            // return true;
            // if x ? y : z
            //return ((intervals.B1 > intervals.A2) || (intervals.B2 < intervals.A1)) 
            //    ? false 
            //    : true;
            return !((intervals.B1 > intervals.A2) || (intervals.B2 < intervals.A1));
        }
    }
}
