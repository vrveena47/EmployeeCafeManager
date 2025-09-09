using CafeEmployeeManager.Server.Application.Dto;
using CafeEmployeeManager.Server.Application.Interfaces;
using CafeEmployeeManager.Server.Domain.Entities;
using CafeEmployeeManager.Server.Infrastructure.Repositories;
using MediatR;
using System.Text.Json.Serialization;

namespace CafeEmployeeManager.Server.Application.Commands.Employee
{
    public record UpdateEmployeeCommand(
     string EmployeeId,
     string Name,
     string EmailAddress,
     string PhoneNumber,
     string Gender,
     Guid? CafeId
 ) : IRequest<EmployeeDto>;
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _repo;
        private readonly ICafeRepository _cafeRepo;
        public UpdateEmployeeCommandHandler(IEmployeeRepository repo, ICafeRepository cafeRepo)
        {
            _repo = repo;
            _cafeRepo = cafeRepo;
        }

        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repo.GetByIdAsync(request.EmployeeId);
            if (employee == null) return null!;

            employee.Name = request.Name;
            employee.EmailAddress = request.EmailAddress;
            employee.PhoneNumber = request.PhoneNumber;
            employee.Gender = request.Gender;
            // Remove existing EmployeeCafe relationship if exists
            if (employee.EmployeeCafe != null)
            {
                await _repo.DeleteEmployeeCafeAsync(employee.EmployeeCafe); // new method in repo
                employee.EmployeeCafe = null;
            }
            if (request.CafeId.HasValue)
            {
                var cafe = await _cafeRepo.GetByIdAsync(request.CafeId.Value);
                if (cafe != null)
                {
                    employee.EmployeeCafe = new EmployeeCafe
                    {
                        EmployeeId = employee.EmployeeId,
                        CafeId = cafe.CafeId,
                        StartDate = DateTime.UtcNow
                    };
                }
            }

            await _repo.UpdateAsync(employee);
            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                EmailAddress = employee.EmailAddress,
                PhoneNumber = employee.PhoneNumber,
                Gender = employee.Gender,
                DaysWorked = employee.EmployeeCafe != null ? (DateTime.UtcNow - employee.EmployeeCafe.StartDate).Days : 0
            };
        }
    }
}