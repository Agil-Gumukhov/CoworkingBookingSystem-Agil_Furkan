using Coworking.APP.Features.Amenities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coworking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AmenitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AmenitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<AmenityQueryResponse>>> Get()
        {
            var result = await _mediator.Send(new AmenityQueryAllRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AmenityQueryResponse>> Get(int id)
        {
            try
            {
                var result = await _mediator.Send(new AmenityQueryRequest { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AmenityCreateResponse>> Post([FromBody] AmenityCreateRequest request)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] AmenityUpdateRequest request)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new AmenityDeleteRequest { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
