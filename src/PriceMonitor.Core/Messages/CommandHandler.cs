using FluentValidation.Results;
using PriceMonitor.Core.Data;
using System.Threading.Tasks;

namespace PriceMonitor.Core.Messages
{
    public abstract class CommandHandler
    {
        public ValidationResult ValidationResult { get; set; }

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected async Task<ValidationResult> Persist(IUnitOfWork unitOfWork)
        {
            if (!await unitOfWork.Commit()) AddError("Error while persisting data.");

            return ValidationResult;
        }
    }
}
