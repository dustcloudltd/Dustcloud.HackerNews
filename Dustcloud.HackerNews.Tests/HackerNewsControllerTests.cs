using Dustcloud.HackerNews.Common.Model;
using Dustcloud.HackerNews.Controllers;
using Dustcloud.HackerNews.Repository.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dustcloud.HackerNews.Tests;

[TestClass]
public class HackerNewsControllerTests
{
    [TestMethod]
    public  void WhenCacheAndServiceDataComparisonIsTrue_GetStatus200()
    {
        object itemsList = GetDustcloudData();

        var memoryCacheMock = new Mock<IMemoryCache>();
        memoryCacheMock.Setup(s => s.TryGetValue("NewsStory", out itemsList))
            .Returns(true);

        var service = new Mock<IHackerNewsService>();
        service.Setup(s => s.GetAllTopStoryIdsAsync())
            .ReturnsAsync(new []{1, 2, 3, 4,5});

        var controller = new HackerNewsControllerBuilder()
            .With(memoryCacheMock.Object)
            .With(service.Object)
            .Build();

        var result = controller.GetHackerNews(5).Result as OkObjectResult;
        var data = result.Value as IEnumerable<DustcloudNewsItem>;
        Assert.AreEqual(200, result.StatusCode);

        Assert.AreEqual(3, data.ToList()[2].Id);
        Assert.AreEqual("Title2", data.ToList()[3].Title); //Checks Item ID =2 is at 4th position

    }

    [TestMethod]
    public void WhenCacheHasNoData_DataIsPopulated_GetStatus200() 
    {
        object itemsList = null;
        var cacheEntry = new Mock<ICacheEntry>();
        
        var memoryCacheMock = new Mock<IMemoryCache>();
        memoryCacheMock.Setup(s => s.CreateEntry("NewsStory"))
            .Returns(() =>
            {
                itemsList = GetDustcloudData();
                cacheEntry.Setup(s => s.Value)
                    .Returns(itemsList);
                memoryCacheMock.Setup(s => s.TryGetValue("NewsStory", out itemsList))
                    .Returns(true);
                return cacheEntry.Object;
            });
            
        memoryCacheMock.Setup(s => s.TryGetValue("NewsStory", out itemsList))
            .Returns(false);
        
        var service = new Mock<IHackerNewsService>();
        service.Setup(s => s.GetAllTopStoryIdsAsync())
            .ReturnsAsync(new[] { 1, 2, 3, 4, 5 });
        service.Setup(s => s.GetNewsItemsByIds(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(GetHackerNewsData());

        var controller = new HackerNewsControllerBuilder()
            .With(memoryCacheMock.Object)
            .With(service.Object)
            .Build();

        var result = controller.GetHackerNews(5).Result as OkObjectResult;
        var data = result.Value as IEnumerable<DustcloudNewsItem>;
        Assert.AreEqual(200, result.StatusCode);

        Assert.AreEqual(3, data.ToList()[2].Id);
        Assert.AreEqual("Title2", data.ToList()[3].Title); //Checks Item ID =2 is at 4th position

    }

    [TestMethod]
    //[ExpectedException(typeof(Exception), "Cache buggy")]
    public void WhenExceptionThrown_VerifyLogged_GetBadRequest()
    {
        var memoryCacheMock = new Mock<IMemoryCache>();
        object itemsList = null;
        memoryCacheMock.Setup(s => s.TryGetValue("NewsStory", out itemsList))
            .Returns(false);
        var service = new Mock<IHackerNewsService>();
        service.Setup(s => s.GetAllTopStoryIdsAsync())
            .Throws(new Exception("Cache buggy"));

        var loggerMock = new Mock<ILogger<HackerNewsController>>();
        var controller = new HackerNewsControllerBuilder()
            .With(loggerMock.Object)
            .With(memoryCacheMock.Object)
            .With(service.Object)
            .Build();

        var result = controller.GetHackerNews(5).Result as BadRequestObjectResult;
        Assert.AreEqual(400, result.StatusCode);
        loggerMock.Verify(s => s.Log(LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((@object, @type) => @type.Name == "FormattedLogValues"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()));
    }

    private List<DustcloudNewsItem> GetDustcloudData()
    {
        return new List<DustcloudNewsItem>()
        {
            new()
            {
                CommentCount = 1,
                Id = 1,
                PostedBy = "PostedBy1",
                Score = 100,
                Time = DateTime.MinValue,
                Title = "Title1",
                Uri = "Url1"
            },
            new()
            {
                CommentCount = 2,
                Id = 2,
                PostedBy = "PostedBy2",
                Score = 200,
                Time = DateTime.MinValue,
                Title = "Title2",
                Uri = "Url2"
            },
            new()
            {
                CommentCount = 3,
                Id = 3,
                PostedBy = "PostedBy3",
                Score = 300,
                Time = DateTime.MinValue,
                Title = "Title3",
                Uri = "Url3"
            },
            new()
            {
                CommentCount = 4,
                Id = 4,
                PostedBy = "PostedBy4",
                Score = 400,
                Time = DateTime.MinValue,
                Title = "Title4",
                Uri = "Url4"
            },
            new()
            {
                CommentCount = 5,
                Id = 5,
                PostedBy = "PostedBy5",
                Score = 500,
                Time = DateTime.MinValue,
                Title = "Title5",
                Uri = "Url5"
            },
        };
    }

    private List<HackerNewsItem> GetHackerNewsData()
    {
        return new List<HackerNewsItem>()
        {
            new()
            {
                Descendants = 1,
                Id = 1,
                By = "By1",
                Score = 100,
                Time = 1697101200, //2023-10-12 9:00AM
                Title = "Title1",
                Url = "Url1"
            },
            new()
            {
                Descendants = 2,
                Id = 2,
                By = "By2",
                Score = 200,
                Time = 1697101200,
                Title = "Title2",
                Url = "Url2"
            },
            new()
            {
                Descendants = 3,
                Id = 3,
                By = "By3",
                Score = 300,
                Time = 1697101200,
                Title = "Title3",
                Url = "Url3"
            },
            new()
            {
                Descendants = 4,
                Id = 4,
                By = "By4",
                Score = 400,
                Time = 1697101200,
                Title = "Title4",
                Url = "Url4"
            },
            new()
            {
                Descendants = 5,
                Id = 5,
                By = "By5",
                Score = 500,
                Time = 1697101200,
                Title = "Title5",
                Url = "Url5"
            },
        };
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
