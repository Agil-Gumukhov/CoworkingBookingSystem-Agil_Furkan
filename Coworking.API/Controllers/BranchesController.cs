using Coworking.APP.Features.Branches;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coworking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BranchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<BranchQueryResponse>>> Get()
        {
            var result = await _mediator.Send(new BranchQueryAllRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BranchQueryResponse>> Get(int id)
        {
            try
            {
                var result = await _mediator.Send(new BranchQueryRequest { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BranchCreateResponse>> Post([FromBody] BranchCreateRequest request)
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
        public async Task<IActionResult> Put(int id, [FromBody] BranchUpdateRequest request)
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
                var result = await _mediator.Send(new BranchDeleteRequest { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}