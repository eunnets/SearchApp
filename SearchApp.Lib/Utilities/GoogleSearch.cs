using SearchApp.Lib.Models;
using SearchApp.Lib.Models.Validators;
using SearchApp.Lib.Utilities;
using System.Net;

namespace SearchApp
{
    public class GoogleSearch : ISearch
    {
        public const string googleBaseUrl = "https://www.google.com.au";
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;

        public GoogleSearch(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request)
        {
            SearchRequestValidator validator = new SearchRequestValidator();
            if (!validator.Validate(request).IsValid)
            {
                throw new ArgumentException($"Invalid request argument");
            }

            string searchUrl = $"{googleBaseUrl}/search?num={request.MaxNumber}&q={request.Keywords}";
            var httpResponse = await _httpClient.GetAsync(searchUrl);
            SearchResponse response = new SearchResponse
            {
                HighestAppearance = 0,
                NumberOfAppearance = 0,
                Results = null,
            };

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await httpResponse.Content.ReadAsStringAsync();
                string classValue = HtmlParser.GetAttributeValue(responseBody, HtmlElementNames.HTML_TAG_DIV, HtmlElementNames.HTML_ATTRIBUTE_CLASS, request.Url);

                if (string.IsNullOrEmpty(classValue))
                {
                    classValue = HtmlParser.GetAttributeValue(responseBody, HtmlElementNames.HTML_TAG_DIV, HtmlElementNames.HTML_ATTRIBUTE_CLASS);
                }
             
                if (!string.IsNullOrEmpty(classValue))
                {
                    List<SearchResultItem> resultList = new List<SearchResultItem>();
                    var urls = HtmlParser.GetUrls(responseBody, HtmlElementNames.HTML_TAG_DIV, HtmlElementNames.HTML_ATTRIBUTE_CLASS, classValue);
                    for (int index = 0; index < urls.Count; index++)
                    {
                        if (urls[index].Equals(request.Url))
                        {
                            if (response.HighestAppearance == 0)
                            {
                                response.HighestAppearance = index + 1;
                            }
                            response.NumberOfAppearance++;
                        }

                        resultList.Add(new SearchResultItem
                        {
                            Rank = index + 1,
                            Url = urls[index]
                        });
                    }
                    response.Results = resultList;
                }
            }

            return response;
        }
    }
}
