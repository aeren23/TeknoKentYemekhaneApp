using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YemekhaneApp.Application.CQRS.Commands.UserDebt;
using YemekhaneApp.Application.CQRS.Queries.UserDebt;
using YemekhaneApp.Application.CQRS.Queries.UserDebts;

namespace YemekhaneApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDebtController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserDebtController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/UserDebts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUserDebts());
            if (!result.Success)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }
        // GET: api/UserDebts/employee/{employeeId}
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(Guid employeeId)
        {
            var result = await _mediator.Send(new GetUserDebtsByEmployeeId(employeeId));
            if (!result.Success)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }
        // GET: api/UserDebts/employee/{employeeId}/year/{year}/month/{month}
        [HttpGet("employee/{employeeId}/year/{year}/month/{month}")]
        public async Task<IActionResult> GetByEmployeeIdAndMonth(Guid employeeId, int year, int month)
        {
            var result = await _mediator.Send(new GetUserDebtByEmployeeIdAndMonth(employeeId, year, month));
            if (!result.Success)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        // POST: api/UserDebt
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDebtCommand command)
        {
            if (command == null)
                return BadRequest("Geçersiz istek.");

            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByEmployeeIdAndMonth), new { employeeId = command.EmployeeId, year = command.Year, month = command.Month }, id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteUserDebtCommand(id));
            if (!result)
                return NotFound("Kayıt bulunamadı veya silinemedi.");

            return NoContent();
        }

    }
}
