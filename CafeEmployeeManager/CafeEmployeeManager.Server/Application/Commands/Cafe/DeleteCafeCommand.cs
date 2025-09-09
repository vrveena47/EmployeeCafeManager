using CafeEmployeeManager.Server.Application.Interfaces;
using MediatR;

namespace CafeEmployeeManager.Server.Application.Commands.Cafe
{
    public record DeleteCafeCommand(Guid CafeId) : IRequest<bool>;

    public class DeleteCafeCommandHandler : IRequestHandler<DeleteCafeCommand, bool>
    {
        private readonly ICafeRepository _cafeRepo;
        private readonly IEmployeeRepository _employeeRepo;
        public DeleteCafeCommandHandler(ICafeRepository cafeRepo, IEmployeeRepository employeeRepo)
        {
            _cafeRepo = cafeRepo;
            _employeeRepo = employeeRepo;
        }
        public async Task<bool> Handle(DeleteCafeCommand request, CancellationToken cancellationToken)
        {
            var cafe = await _cafeRepo.GetByIdAsync(request.CafeId);
            if (cafe == null) return false;

            // Get all employees under this cafe
            var employees = await _employeeRepo.GetAllAsync(cafe.Name);

            // Delete all employees under this cafe
            foreach (var emp in employees)
            {
                await _employeeRepo.DeleteAsync(emp.EmployeeId);
            }


            await _cafeRepo.DeleteAsync(request.CafeId);
            return true;
        }
    }
}