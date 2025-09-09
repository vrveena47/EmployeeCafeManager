using CafeEmployeeManager.Server.Application.Interfaces;
using CafeEmployeeManager.Server.Domain.Entities;
using CafeEmployeeManager.Server.Infrastructure.Repositories;
using MediatR;
using System.Text;

namespace CafeEmployeeManager.Server.Application.Commands.Employee
{
    public record CreateEmployeeCommand(
        string Name,
        string EmailAddress,
        string PhoneNumber,
        string Gender,
        Guid? CafeId
    ) : IRequest<string>; // return generated EmployeeId

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, string>
    {
        private readonly IEmployeeRepository _repo;
        private readonly ICafeRepository _cafeRepo;
        private static readonly Random _rng = new();

        public CreateEmployeeCommandHandler(IEmployeeRepository repo, ICafeRepository cafeRepo)
        {
            _repo = repo;
            _cafeRepo = cafeRepo;
        }

        public async Task<string> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Generate unique EmployeeId
            var newEmployeeId = "UI" + Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper();


            var employee = new Domain.Entities.Employee
            {
                EmployeeId = newEmployeeId,
                Name = request.Name,
                EmailAddress = request.EmailAddress,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender
            };

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

            await _repo.AddAsync(employee);
            return employee.EmployeeId;
        }

       
    }
}
