using SearchApp.Lib.Models;
using SearchApp.Lib.Models.Validators;
using Shouldly;

namespace SearchApp.Lib.Tests.Models.Validators
{
    public class SearchRequestValidatorTests
    {
        [Theory]
        [InlineData("conveyancing software", "www.smokeball.com.au", 100, true)]
        [InlineData("", "www.smokeball.com.au", 100, false)]
        [InlineData(null, "www.smokeball.com.au", 100, false)]
        [InlineData("conveyancing software", "", 100, false)]
        [InlineData("conveyancing software", null, 100, false)]
        [InlineData("conveyancing software", "www.smokeball.com.au", 0, false)]
        public void When_request_is_given_it_should_validate_as_expected(
          string keywords,
          string url,
          int maxNumber,
          bool isValid)
        {
            SearchRequest request = new SearchRequest
            {
                Keywords = keywords,
                Url = url,
                MaxNumber = maxNumber,
            };

            SearchRequestValidator validator = new SearchRequestValidator();
            var result = validator.Validate(request);
            result.IsValid.ShouldBe(isValid);
        }
    }
}
