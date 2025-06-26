using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YemekhaneApp.Application.CQRS.Commands.MealRecord;
using YemekhaneApp.Application.CQRS.Queries.MealRecord;

namespace YemekhaneApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealRecordController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MealRecordController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllMealRecords()
        {
            var query = new GetAllMealRecordsQuery();
            var result = await _mediator.Send(query);
            if (result == null || !result.Success)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Value);
        }
        [HttpGet("by-date/{id}")]
        public async Task<IActionResult> GetMealRecordByDate(Guid id)
        {
            var query = new GetMealsByDateWithEmployeeQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMealRecordsByIdWithEmployee(Guid id)
        {
            var query = new GetMealRecordByIdWithEmployeeQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetMealRecordsByEmployeeId(Guid employeeId)
        {
            var query = new GetMealRecordsByEmployeeIdQuery(employeeId);
            var result = await _mediator.Send(query);
            if (result == null || !result.Success)
            {
                return NotFound(result?.ErrorMessage);
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMealRecord([FromBody] CreateMealRecordCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid meal record data.");
            }
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMealRecord([FromBody] UpdateMealRecordCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid meal record data.");
            }
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMealRecord(Guid id)
        {
            var command = new DeleteMealRecordCommand(id);
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return NoContent();
        }
        [HttpDelete("year/{year}/month/{month}")]
        public async Task<IActionResult> DeleteAllMealRecordsByMonth(int year, int month)
        {
            var command = new DeleteAllMealRecordByMonthCommand(month, year);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();

        }
    }
}
