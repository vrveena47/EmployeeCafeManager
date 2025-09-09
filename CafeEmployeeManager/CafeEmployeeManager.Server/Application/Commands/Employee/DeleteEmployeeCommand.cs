using CafeEmployeeManager.Server.Application.Interfaces;
using CafeEmployeeManager.Server.Infrastructure.Repositories;
using MediatR;

namespace CafeEmployeeManager.Server.Application.Commands.Employee
{
    public record DeleteEmployeeCommand(string EmployeeId) : IRequest<bool>;

    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _repo;
        public DeleteEmployeeCommandHandler(IEmployeeRepository repo) => _repo = repo;

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repo.GetByIdAsync(request.EmployeeId);
            if (employee == null) return false;

            await _repo.DeleteAsync(request.EmployeeId);
            return true;
        }
    }
}