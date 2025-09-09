using CafeEmployeeManager.Server.Application.Dto;
using CafeEmployeeManager.Server.Application.Interfaces;
using CafeEmployeeManager.Server.Infrastructure.Repositories;
using MediatR;

namespace CafeEmployeeManager.Server.Application.Queries.Employee
{

    public record GetEmployeeByIdQuery(string EmployeeId) : IRequest<EmployeeDto?>;

    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
    {
        private readonly IEmployeeRepository _repo;
        public GetEmployeeByIdQueryHandler(IEmployeeRepository repo) => _repo = repo;

        public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var e = await _repo.GetByIdAsync(request.EmployeeId);
            if (e == null) return null;

            return new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                EmailAddress = e.EmailAddress,
                PhoneNumber = e.PhoneNumber,
                Gender = e.Gender,
                Cafe = e.EmployeeCafe?.Cafe.Name ?? "",
                CafeId = e.EmployeeCafe?.Cafe.CafeId ?? null,
                DaysWorked = e.EmployeeCafe != null ? Math.Max(0, (DateTime.UtcNow - e.EmployeeCafe.StartDate).Days) : 0
            };
        }
    }
}