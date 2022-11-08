using SearchApp.Lib.Utilities;
using Shouldly;

namespace SearchApp.Lib.Tests.Utilities
{
    public class HtmlParserTests
    {
        [Theory]
        [InlineData("div", "class", "BNeawe UPmit AP7Wnd lRVwie", 100)]
        [InlineData("div", "class", "", 0)]
        [InlineData("div", "id", "", 0)]
        [InlineData("head", "class", "", 0)]
        public void When_conditions_are_given_GetUrls_should_work_as_expected(
           string tagName,
           string attributeName,
           string attributeValue,
           int numberOfUrls)
        {
            string html = File.ReadAllText(Path.GetFullPath(@"./Utilities/TestData/Test.html"));
            var response = HtmlParser.GetUrls(html, tagName, attributeName, attributeValue);
           
            response.Count.ShouldBe(numberOfUrls);
        }

        [Theory]
        [InlineData("div", "class", "www.smokeball.com.au", "BNeawe UPmit AP7Wnd lRVwie")]
        [InlineData("div", "class", "www.leapconveyancer.com.au", "BNeawe UPmit AP7Wnd lRVwie")]
        [InlineData("div", "class", "", "BNeawe UPmit AP7Wnd lRVwie")]
        [InlineData("div", "id", "", "")]
        [InlineData("head", "class", "", "")]
        public void When_conditions_are_given_GetAttributeValue_should_work_as_expected(
           string tagName,
           string attributeName,
           string url,
           string attributeValue)
        {
            string html = File.ReadAllText(Path.GetFullPath(@"./Utilities/TestData/Test.html"));
            var response = HtmlParser.GetAttributeValue(html, tagName, attributeName, url);

            response.ShouldBe(attributeValue);
        }
    }
}
