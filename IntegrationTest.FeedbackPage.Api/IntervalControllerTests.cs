using FeedbackPage.Api.Controllers;
using FeedbackPage.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntegrationTest.FeedbackPage.Api
{
    [TestClass]
    public class IntervalControllerTests
    {
        [DataTestMethod]
        [DataRow(-2, 4, 2, 6, true)]
        [DataRow(-2, 5, 10, 15, false)]
        [DataRow(5, 10, 0, 5, true)]
        [DataRow(1, 10, 3, 7, true)]
        public void Test_CheckOverlap(int a1, int a2, int b1, int b2, bool expectedResult)
        {
            // Arrange
            var testInterval = new IntervalsSubmission
            {
                A1 = a1,
                A2 = a2,
                B1 = b1,
                B2 = b2
            };

            var controllerUndertest = new IntervalController();

            // Execute
            var response = controllerUndertest.CheckOverlap(testInterval);
            
            // Assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));

            var okResult = response as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(GenericDataResponse<bool>));

            var resultValue = okResult.Value as GenericDataResponse<bool>;
            Assert.IsTrue(resultValue.Success, "The response was not successful.");
            Assert.AreEqual(expectedResult, resultValue.Data, $"Expected {expectedResult} but instead received {resultValue.Data} for interval overlap result.");
        }
    }
}
