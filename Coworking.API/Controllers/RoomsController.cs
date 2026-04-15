using Coworking.APP.Features.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coworking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomQueryResponse>>> Get()
        {
            var result = await _mediator.Send(new RoomQueryAllRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomQueryResponse>> Get(int id)
        {
            try
            {
                var result = await _mediator.Send(new RoomQueryRequest { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoomCreateResponse>> Post([FromBody] RoomCreateRequest request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RoomUpdateRequest request)
        {
            try
            {
                request.Id = id;
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new RoomDeleteRequest { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}