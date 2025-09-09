using CafeEmployeeManager.Server.Application.Dto;
using CafeEmployeeManager.Server.Application.Interfaces;
using CafeEmployeeManager.Server.Infrastructure.Repositories;
using MediatR;

namespace CafeEmployeeManager.Server.Application.Queries.Employee
{
    public record GetAllEmployeesQuery(string? CafeName) : IRequest<List<EmployeeDto>>;

    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IEmployeeRepository _repo;
        public GetAllEmployeesQueryHandler(IEmployeeRepository repo) => _repo = repo;

        public async Task<List<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _repo.GetAllAsync(request.CafeName);

            return employees.Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                EmailAddress = e.EmailAddress,
                PhoneNumber = e.PhoneNumber,
                Gender = e.Gender,
                Cafe = e.EmployeeCafe?.Cafe.Name ?? "",
                DaysWorked = e.EmployeeCafe != null ? Math.Max(0, (DateTime.UtcNow - e.EmployeeCafe.StartDate).Days) : 0
            }).ToList();
        }
    }
}