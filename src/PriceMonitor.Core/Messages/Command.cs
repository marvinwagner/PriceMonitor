using FluentValidation.Results;
using MediatR;

namespace PriceMonitor.Core.Messages
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        public ValidationResult ValidationResult { get; set; }

        public virtual bool IsValid()
        {
            return true;
        }
    }
}
