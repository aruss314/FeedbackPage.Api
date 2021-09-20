using FeedbackPage.Api.Extensions;
using FeedbackPage.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.FeedbackPage.Api
{
    [TestClass]
    public class IntervalExtensionTests
    {
        [DataTestMethod]
        [DataRow(-2,4,2,6, true)]
        [DataRow(-2,5,10,15, false)]
        [DataRow(5,10,0,5, true)]
        [DataRow(1,10,3,7, true)]
        public void Test_CheckOverlap(int a1, int a2, int b1, int b2, bool expectedResult)
        {
            var testInterval = new IntervalsSubmission {
                A1 = a1,
                A2 = a2,
                B1 = b1,
                B2 = b2
            };
            bool overlapResult = testInterval.CheckOverlap();
            Assert.AreEqual(expectedResult, overlapResult, $"Expected {expectedResult} but instead received {overlapResult} for interval overlap result.");
        }
    }
}
