using FluentValidation;

namespace SearchApp.Lib.Models.Validators
{
    public class SearchRequestValidator : AbstractValidator<SearchRequest>
    {
        public SearchRequestValidator()
        {
            RuleFor(request => request.Keywords).NotEmpty();
            RuleFor(request => request.Url).NotEmpty();
            RuleFor(request => request.MaxNumber).GreaterThan(0);
        }
    }
}
