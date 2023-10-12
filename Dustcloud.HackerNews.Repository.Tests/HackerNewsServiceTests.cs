using Dustcloud.HackerNews.Repository.Services;
using Moq;

namespace Dustcloud.HackerNews.Repository.Tests;

[TestClass]
public class HackerNewsServiceTests
{
    [TestMethod]
    public async Task WhenDataRequestedFromProxy_CheckDataReturnedCorrectly()
    {
        var proxy = new Mock<IHttpClientProxy>();
        proxy.Setup(s => s.GetStringAsync("item/1.json"))
            .ReturnsAsync(@"{
                                    'by' : 'dhouston',
                                    'descendants' : 2,
                                    'id' : 1,
                                    'kids' : [1,2 ],
                                    'score' : 100,
                                    'time' : 1175714200,
                                    'title' : 'Test 1',
                                    'type' : 'story',
                                    'url' : 'http://google.com'
                                }");
        proxy.Setup(s => s.GetStringAsync("item/2.json"))
            .ReturnsAsync(@"{
                                    'by' : 'greg',
                'descendants' : 2,
                'id' :2,
                'kids' : [3,4 ],
                'score' : 200,
                'time' : 1175714201,
                'title' : 'Test 2',
                'type' : 'story',
                'url' : 'http://bing.com'
                                }");
        var service = new HackerNewsService(proxy.Object);

        var data = await service.GetNewsItemsByIds(new[] { 1, 2 });
        var listedData = data.ToList();
        Assert.AreEqual(2, data.Count());
        Assert.AreEqual(1175714200, listedData[0].Time);
        Assert.AreEqual(100, listedData[0].Score);

        Assert.AreEqual("Test 2", listedData[1].Title);
        Assert.AreEqual("http://bing.com", listedData[1].Url);
    }
}
