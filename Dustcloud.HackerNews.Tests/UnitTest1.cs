using Dustcloud.HackerNews.Controllers;
using Dustcloud.HackerNews.Repository.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dustcloud.HackerNews.Tests;

[TestClass]
public class HackerNewsControllerTests
{
    [TestMethod]
    public async void TestMethod1()
    {
        var memoryCacheMock = new Mock<IMemoryCache>();
        var service = new Mock<IHackerNewsService>();

        var controller = new HackerNewsControllerBuilder()
            .With(memoryCacheMock.Object)
            .With(service.Object)
            .Build();

        var b = await controller.GetHackerNews(5);


    }

    private class HackerNewsControllerBuilder
    {
        private IHackerNewsService _hackerNewsService;
        private IMemoryCache _memoryCache;
        private ILogger<HackerNewsController> _logger;

        public HackerNewsController Build()
        {
            return new HackerNewsController(_hackerNewsService ?? Mock.Of<IHackerNewsService>(),
                _memoryCache ?? Mock.Of<IMemoryCache>(),
                _logger ?? Mock.Of<ILogger<HackerNewsController>>());
        }

        public HackerNewsControllerBuilder With(IHackerNewsService value)
        {
            _hackerNewsService = value;
            return this;
        }
        public HackerNewsControllerBuilder With(IMemoryCache value)
        {
            _memoryCache = value;
            return this;
        }
        public HackerNewsControllerBuilder With(ILogger<HackerNewsController> value)
        {
            _logger = value;
            return this;
        }
    }
}
