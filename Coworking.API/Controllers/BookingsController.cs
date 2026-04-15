using Coworking.APP.Features.Bookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coworking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookingQueryResponse>>> Get()
        {
            var result = await _mediator.Send(new BookingQueryAllRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingQueryResponse>> Get(int id)
        {
            try
            {
                var result = await _mediator.Send(new BookingQueryRequest { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookingCreateResponse>> Post([FromBody] BookingCreateRequest request)
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
        public async Task<IActionResult> Put(int id, [FromBody] BookingUpdateRequest request)
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
                var result = await _mediator.Send(new BookingDeleteRequest { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
