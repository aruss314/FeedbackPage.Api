using FeedbackPage.Dal.Configuration;
using FeedbackPage.Dal.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.FeedbackPage.Api
{
    [TestClass]
    public class WinHistoryTests
    {
        IWinHistoryRepository repoUnderTest;
        private ServiceProvider provider;

        /// <summary>
        /// Runs once for all tests, sets up the service provider for dependency injection
        /// </summary>
        [ClassInitialize]
        public void ClassInit()
        {
            var builder = new ConfigurationBuilder();
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
        [TestMethod]
        public void TestMethod1()
        {
            var record = repoUnderTest.GetWinHistoryRecord(1);
            Assert.IsTrue(false);
        }
        [TestMethod]
        public void TestMethod2()
        {
        }
        [TestMethod]
        public void TestMethod3()
        {
        }
        [TestMethod]
        public void TestMethod4()
        {
        }
    }
}
