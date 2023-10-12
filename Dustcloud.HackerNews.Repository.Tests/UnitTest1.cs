using Dustcloud.HackerNews.Repository.Services;
using Moq;

namespace Dustcloud.HackerNews.Repository.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public  void TestMethod1()
        {
            var proxy = new Mock<IHttpClientProxy>();
            var service = new HackerNewsService(proxy.Object);

            var b =  service.GetAllTopStoriesAsync();
        }
    }
}