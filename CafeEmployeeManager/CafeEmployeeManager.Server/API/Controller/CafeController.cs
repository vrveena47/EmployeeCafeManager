using CafeEmployeeManager.Server.Application.Commands.Cafe;
using CafeEmployeeManager.Server.Application.Dto;
using CafeEmployeeManager.Server.Application.Queries.Cafe;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CafeEmployeeManager.Server.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CafeController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? location)
        {
            var result = await _mediator.Send(new GetAllCafesQuery(location));
            return Ok(result);
        }

        // GET api/cafe/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _mediator.Send(new GetCafeByIdQuery(id));
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // POST api/cafe
        // Accepts CreateCafeCommand as request body (Name, Description, Location, Logo)
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> Create([FromBody] CreateCafeCommand command)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var createdId = await _mediator.Send(command);

        //    // Return 201 with Location header pointing to GET /api/cafe/{id}
        //    return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId });
        //}
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCafeCommand dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            byte[]? logoBytes = null;
            if (dto.LogoFile != null && dto.LogoFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await dto.LogoFile.CopyToAsync(ms);
                logoBytes = ms.ToArray();
            }

            var command = new CreateCafeCommand(
                dto.Name,
                dto.Description,
                dto.Location,
                dto.LogoFile,
                logoBytes
            );

            var createdId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId });
        }

        // PUT api/cafe
        // Accepts UpdateCafeCommand as request body (CafeId, Name, Description, Location, Logo)
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(Guid id,[FromBody] UpdateCafeDto dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);


        //    byte[]? logoBytes = null;
        //    if (dto.LogoFile != null && dto.LogoFile.Length > 0)
        //    {
        //        using var ms = new MemoryStream();
        //        await dto.LogoFile.CopyToAsync(ms);
        //        logoBytes = ms.ToArray();
        //    }
        //    else
        //    {
        //        logoBytes = dto.Logo;
        //    }

        //    var command = new UpdateCafeCommand(
        //        id,
        //        dto.Name,
        //        dto.Description,
        //        dto.Location,
        //        logoBytes
        //    );

        //    var updatedCafe = await _mediator.Send(command);
        //    if (updatedCafe == null) return NotFound();

        //    return Ok(updatedCafe);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCafeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            byte[]? logoBytes = null;
            if (dto.LogoFile != null && dto.LogoFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await dto.LogoFile.CopyToAsync(ms);
                logoBytes = ms.ToArray();
            }

            var command = new UpdateCafeCommand(
                id,
                dto.Name,
                dto.Description,
                dto.Location,
                logoBytes
            );

            var updatedCafe = await _mediator.Send(command);
            if (updatedCafe == null) return NotFound();

            return Ok(updatedCafe);
        }

        // DELETE api/cafe/{id}
        // Deleting a cafe should also delete employees (cascade)
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteCafeCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }
    }
}