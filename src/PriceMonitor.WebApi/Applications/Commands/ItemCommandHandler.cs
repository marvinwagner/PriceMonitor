using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PriceMonitor.Core.Messages;
using PriceMonitor.WebApi.Data.Repositories;
using PriceMonitor.WebApi.Models;
using PriceMonitor.WebApi.Applications.Events;

namespace PriceMonitor.WebApi.Applications.Commands
{
    public class ItemCommandHandler : CommandHandler,
        IRequestHandler<CreateItemCommand, ValidationResult>,
        IRequestHandler<UpdatePriceCommand, ValidationResult>
    {
        private readonly IItemRepository _itemRepository;

        public ItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ValidationResult> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var item = new Item(request.Name, request.Url);

            var existingItem = await _itemRepository.FindByUrl(item.Url);
            if (existingItem != null)
            {
                AddError("Link already registered.");
                return ValidationResult;
            }

            _itemRepository.Create(item);

            item.AddEvent(new ItemCreatedEvent(item.Id, request.Name, request.Url));

            return await Persist(_itemRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var existingItem = await _itemRepository.FindById(request.ItemId);
            if (existingItem == null)
            {
                AddError("Item not found.");
                return ValidationResult;
            }

            existingItem.UpdateValues(request.InCashValue, request.NormalValue, request.FullValue);
            existingItem.SetAvailability(request.Available);

            var history = new ItemHistory(request.ItemId, request.InCashValue, request.NormalValue, request.FullValue, request.Available);
            _itemRepository.AddHistory(history);

            existingItem.AddEvent(new PriceUpdatedEvent(request.ItemId, request.InCashValue, request.NormalValue, request.FullValue, request.Available));

            return await Persist(_itemRepository.UnitOfWork);
        }
    }
}