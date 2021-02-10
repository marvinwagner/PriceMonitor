using FluentValidation;
using System.Text.RegularExpressions;

namespace PriceMonitor.Core.Validation
{
    public static class UrlValidation
    {
        private const string URL_REGEX = @"[-a-zA-Z0-9@:%.+~#=]{1,256}.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_+.~#?&//=]*)";

        public static IRuleBuilderInitial<T, string> UrlMustBeValid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((url, context) => {
                if (!Regex.IsMatch(url, URL_REGEX))
                    context.AddFailure("Invalid url");
            });
        }
    }
}