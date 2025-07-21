using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YemekhaneApp.Application.CQRS.Commands.Employee;
using YemekhaneApp.Application.CQRS.Queries.Empolyee;

namespace YemekhaneApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var query = new GetAllEmployees();
            var result = await _mediator.Send(query);
            return Ok(result.Value);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var query = new GetEmployeeByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid employee data.");
            }
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid employee data.");
            }
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var command = new DeleteEmployeeCommand()
            {
                Id = id
            };
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return NoContent(); // 204 No Content
        }
    }
}
