using FeedbackPage.Dal.Configuration;
using FeedbackPage.Dal.Models;
using FeedbackPage.Dal.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTest.FeedbackPage.Api
{
    [TestClass]
    public class WinHistoryTests
    {
        private static IWinHistoryRepository repoUnderTest;
        private static ServiceProvider provider;

        private static List<int> testDataCollection = new List<int>();

        /// <summary>
        /// Runs once for all tests, sets up the service provider for dependency injection
        /// </summary>
        [ClassInitialize]
        public static void ClassInit(TestContext ctx)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(@"c:\temp\.FeedbackPage.Api\appsettings.json", true);
            var configuration = builder.Build();
            IServiceCollection services = new ServiceCollection();
            services.AddWinHistoryRepository(configuration);
            provider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Runs once for each test, creates a new repo under test for each test.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            repoUnderTest = provider.GetRequiredService<IWinHistoryRepository>();
        }

        [TestCleanup]
        public async Task CleanupTests()
        {
            var operations = 0;
            for (int i=0; i < testDataCollection.Count; i++)
            {
                operations += await repoUnderTest.DeleteWinHistoryRecord(testDataCollection[i]);
            }
            if (operations != testDataCollection.Count) throw new Exception($"The number of rows deleted did not match the number of records that needed deletion. IDs {string.Join(",",testDataCollection)}");
            testDataCollection = new List<int>();
        }

        [TestMethod]
        public async Task CreateWinRecord()
        {
            int testRecordId = await repoUnderTest.CreateWinHistoryRecord(new WinHistoryRecord
            {
                PlayerName = "TestPlayer1",
                PlayerScore = new Random().Next(),
                TimeOfVictoryUtc = DateTime.UtcNow
            });
            testDataCollection.Add(testRecordId);
            await Task.Delay(1000);
            Assert.IsTrue(testRecordId > 0);
        }
        [TestMethod]
        public async Task ReadWinRecord()
        {
            // setup
            int expectedScore = new Random().Next();
            string expectedName = "TestPlayer2";
            DateTime expectedWinTime = DateTime.UtcNow;

            int testRecordId = await repoUnderTest.CreateWinHistoryRecord(new WinHistoryRecord
            {
                PlayerName = expectedName,
                PlayerScore = expectedScore,
                TimeOfVictoryUtc = expectedWinTime
            });
            testDataCollection.Add(testRecordId);

            // test
            await Task.Delay(1000);
            var record = await repoUnderTest.GetWinHistoryRecord(testRecordId);
            await Task.Delay(1000);
            // assert
            Assert.AreEqual(testRecordId, record.Id);
            Assert.AreEqual(expectedScore, record.PlayerScore);
            Assert.AreEqual(expectedName, record.PlayerName);
            Assert.IsNotNull(record.TimeOfVictoryUtc);
        }
        [TestMethod]
        public async Task ReadMultipleWinRecords()
        {
            // setup
            int expectedScore1 = new Random().Next();
            int expectedScore2 = new Random().Next();
            string expectedName = "TestPlayer3";
            DateTime expectedWinTime = DateTime.UtcNow;

            int testRecordId1 = await repoUnderTest.CreateWinHistoryRecord(new WinHistoryRecord
            {
                PlayerName = expectedName,
                PlayerScore = expectedScore1,
                TimeOfVictoryUtc = expectedWinTime
            });
            testDataCollection.Add(testRecordId1);
            await Task.Delay(1000);
            int testRecordId2 = await repoUnderTest.CreateWinHistoryRecord(new WinHistoryRecord
            {
                PlayerName = expectedName,
                PlayerScore = expectedScore2,
                TimeOfVictoryUtc = expectedWinTime
            });
            testDataCollection.Add(testRecordId2);
            await Task.Delay(1000);
            var recordsRequest = new WinHistoryRecordRequest { PageNumber = 0, PageSize = 2, PlayerName = expectedName };

            // test
            var records = (await repoUnderTest.GetWinHistoryRecords(recordsRequest)).ToList();
            await Task.Delay(1000);
            // assert
            Assert.AreEqual(2,records.Count);

            var resultRecord1 = records.FirstOrDefault(record => record.PlayerScore == expectedScore1);
            Assert.IsNotNull(resultRecord1);

            Assert.AreEqual(testRecordId1, resultRecord1.Id);
            Assert.AreEqual(expectedScore1, resultRecord1.PlayerScore);
            Assert.AreEqual(expectedName, resultRecord1.PlayerName);
            Assert.IsNotNull(resultRecord1.TimeOfVictoryUtc);

            var resultRecord2 = records.FirstOrDefault(record => record.PlayerScore == expectedScore2);
            Assert.IsNotNull(resultRecord2);

            Assert.AreEqual(testRecordId2, resultRecord2.Id);
            Assert.AreEqual(expectedScore2, resultRecord2.PlayerScore);
            Assert.AreEqual(expectedName, resultRecord2.PlayerName);
            Assert.IsNotNull(resultRecord2.TimeOfVictoryUtc);



        }
        [TestMethod]
        public async Task DeleteWinRecords()
        {
            // setup
            int expectedScore = new Random().Next();
            string expectedName = "TestPlayer4";
            DateTime expectedWinTime = DateTime.UtcNow;
            int expectedOperations = 1;

            int testRecordId = await repoUnderTest.CreateWinHistoryRecord(new WinHistoryRecord
            {
                PlayerName = expectedName,
                PlayerScore = expectedScore,
                TimeOfVictoryUtc = expectedWinTime
            });
            testDataCollection.Add(testRecordId);
            await Task.Delay(1000);
            var record = await repoUnderTest.GetWinHistoryRecord(testRecordId);
            await Task.Delay(1000);
            // test
            var operations = await repoUnderTest.DeleteWinHistoryRecord(testRecordId);
            await Task.Delay(1000);
            var recordfail = await repoUnderTest.GetWinHistoryRecord(testRecordId);
            await Task.Delay(1000);
            // assert

            Assert.AreEqual(expectedOperations, operations);
            Assert.IsNull(recordfail);
            testDataCollection = new List<int>();

            // These are probably not actually necessary or related to record deletion and coudl be removed
            Assert.AreEqual(testRecordId, record.Id);
            Assert.AreEqual(expectedScore, record.PlayerScore);
            Assert.AreEqual(expectedName, record.PlayerName);
            Assert.IsNotNull(record.TimeOfVictoryUtc);

        }

    }
}
