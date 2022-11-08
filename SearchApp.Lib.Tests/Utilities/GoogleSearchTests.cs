using Moq;
using Moq.Protected;
using SearchApp.Lib.Models;
using Shouldly;
using System.Net;

namespace SearchApp.Lib.Tests.Utilities
{
    public class GoogleSearchTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;

        public GoogleSearchTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task When_SearchRequest_is_invalid_Search_should_work_as_expected()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(HttpClient.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText(Path.GetFullPath(@"./Utilities/TestData/Test.html")))
                });
            _mockHttpClientFactory.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            GoogleSearch googleSearch = new GoogleSearch(_mockHttpClientFactory.Object);
            SearchRequest request = new SearchRequest
            {
                Keywords = "conveyancing software",
                Url = "",
                MaxNumber = 0
            };
            await googleSearch.SearchAsync(request).ShouldThrowAsync<ArgumentException>("Invalid request argument");
        }

        [Theory]
        [InlineData("www.mock.com", 0, 0)]
        [InlineData("www.smokeball.com.au", 1, 1)]
        [InlineData("www.trisearch.com.au", 3, 2)]
        [InlineData("www.leap.com.au", 8, 1)]
        [InlineData("www.infotrack.com.au", 29, 1)]
        public async Task When_SearchRequest_is_valid_Search_should_work_as_expected(
            string url,
            int highestAppearance,
            int numberOfAppearance)
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(HttpClient.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText(Path.GetFullPath(@"./Utilities/TestData/Test.html")))
                });
            _mockHttpClientFactory.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            GoogleSearch googleSearch = new GoogleSearch(_mockHttpClientFactory.Object);
            SearchRequest request = new SearchRequest
            {
                Keywords = "conveyancing software",
                Url = url,
                MaxNumber = 100
            };
            var response = await googleSearch.SearchAsync(request);
            response.HighestAppearance.ShouldBe(highestAppearance);
            response.NumberOfAppearance.ShouldBe(numberOfAppearance);
        }
    }
}
