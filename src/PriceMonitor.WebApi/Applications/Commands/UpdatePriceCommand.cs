using FluentValidation;
using PriceMonitor.Core.Messages;
using System;

namespace PriceMonitor.WebApi.Applications.Commands
{
    public class UpdatePriceCommand : Command
    {
        public Guid ItemId { get; private set; }
        public decimal InCashValue { get; private set; }
        public decimal NormalValue { get; private set; }
        public decimal FullValue { get; private set; }
        public bool IsAvailable { get; private set; }

        public UpdatePriceCommand(Guid itemId, decimal inCashValue, decimal normalValue, decimal fullValue, bool isAvailable)
        {
            AggregateId = itemId;
            ItemId = itemId;
            InCashValue = inCashValue;
            NormalValue = normalValue;
            FullValue = fullValue;
            IsAvailable = isAvailable;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdatePriceValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class UpdatePriceValidation : AbstractValidator<UpdatePriceCommand>
        {
            public UpdatePriceValidation()
            {
                RuleFor(c => c.ItemId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("ItemId cannot be empty");
            }
        }
    }
}