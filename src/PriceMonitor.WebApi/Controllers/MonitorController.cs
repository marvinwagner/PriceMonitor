using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceMonitor.WebApi.Applications.Commands;
using PriceMonitor.WebApi.Data.Repositories;
using PriceMonitor.WebApi.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitor.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IItemRepository _repository;
        private readonly ILogger<MonitorController> _logger;

        public MonitorController(ILogger<MonitorController> logger, IItemRepository repository, IMediator mediator)
        {
            _logger = logger;
            _repository = repository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _repository.ListAll();
            
            return Ok(list.Select(i => new ResultItemViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Url = i.Url,
                CurrentInCashValue = i.CurrentInCashValue,
                CurrentNormalValue = i.CurrentNormalValue,
                CurrentFullValue = i.CurrentFullValue,
                IsAvailable = i.IsAvailable
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ItemViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.Select(x => string.Join(",", x.Errors.Select(e => e.ErrorMessage))));
            _logger.LogInformation($"Creating item {model.Name}");

            var result = await _mediator.Send(new CreateItemCommand(model.Name, model.Url));

            return Ok(result.IsValid);
        }
    }
}
