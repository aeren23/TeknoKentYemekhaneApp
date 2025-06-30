using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemekhaneApp.Application.CQRS.Commands.Extra;
using YemekhaneApp.Application.CQRS.Queries.Extra;

namespace YemekhaneApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExtraController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExtraController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Extra
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllExtrasQuery());
            if (!result.Success)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        // GET: api/Extra/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetExtraByIdQuery(id));
            if (!result.Success)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        // POST: api/Extra
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExtraCommand command)
        {
            if (command == null)
                return BadRequest("Geçersiz istek.");

            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
        }

        // PUT: api/Extra/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateExtraCommand command)
        {
            if (command == null)
                return BadRequest("Geçersiz istek.");

            var result = await _mediator.Send(command);
            if (!result.Success)
                return NotFound(result.ErrorMessage);

            return Ok();
        }

        // DELETE: api/Extra/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteExtraCommand(id));
            if (!result.Success)
                return NotFound(result.ErrorMessage);

            return NoContent();
        }
    }
}