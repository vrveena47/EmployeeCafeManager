using CafeEmployeeManager.Server.Application.Commands.Employee;
using CafeEmployeeManager.Server.Application.Dto;
using CafeEmployeeManager.Server.Application.Queries.Employee;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CafeEmployeeManager.Server.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmployeeController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? cafe)
        {
            var result = await _mediator.Send(new GetAllEmployeesQuery(cafe));
            return Ok(result);
        }

        // GET api/employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var dto = await _mediator.Send(new GetEmployeeByIdQuery(id));
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // POST api/employee
        // Accepts CreateEmployeeCommand in body (EmployeeId, Name, EmailAddress, PhoneNumber, Gender, CafeId?, StartDate?)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Send command → handler will generate unique EmployeeId
            var employeeId = await _mediator.Send(command);

            if (string.IsNullOrEmpty(employeeId))
                return BadRequest(new { error = "Unable to create employee" });

            // Return 201 Created with route to GetById
            return CreatedAtAction(
                nameof(GetById),
                new { id = employeeId },
                new { id = employeeId }
            );
        }

        // PUT api/employee
        // Accepts UpdateEmployeeCommand in body
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id,[FromBody] UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Map DTO to command and enforce immutable EmployeeId
            var command = new UpdateEmployeeCommand(
                EmployeeId: id,              // Route ID, not editable
                Name: dto.Name,
                EmailAddress: dto.EmailAddress,
                PhoneNumber: dto.PhoneNumber,
                Gender: dto.Gender,
                CafeId: dto.CafeId
            );
            var updatedEmployee = await _mediator.Send(command);
            if (updatedEmployee == null) return NotFound();

            return Ok(updatedEmployee);
        }

        // DELETE api/employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _mediator.Send(new DeleteEmployeeCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }
    }
}