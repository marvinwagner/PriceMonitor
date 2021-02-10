using FluentValidation;
using PriceMonitor.Core.Messages;
using PriceMonitor.Core.Validation;

namespace PriceMonitor.WebApi.Applications.Commands
{
    public class CreateItemCommand : Command
    {
        public string Name { get; private set; }
        public string Url { get; private set; }

        public CreateItemCommand(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class CreateItemValidation : AbstractValidator<CreateItemCommand>
        {
            public CreateItemValidation()
            {
                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("Item name cannot be empty");

                RuleFor(c => c.Url)
                    .NotEmpty()
                    .UrlMustBeValid();
            }
        }
    }
}