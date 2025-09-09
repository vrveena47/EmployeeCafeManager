using CafeEmployeeManager.Server.Application.Interfaces;
using MediatR;

namespace CafeEmployeeManager.Server.Application.Commands.Cafe
{
    public record CreateCafeCommand(string Name, string Description, string Location, IFormFile? LogoFile, byte[]? Logo) : IRequest<Guid>;

    public class CreateCafeCommandHandler : IRequestHandler<CreateCafeCommand, Guid>
    {
        private readonly ICafeRepository _repo;
        public CreateCafeCommandHandler(ICafeRepository repo) => _repo = repo;

        public async Task<Guid> Handle(CreateCafeCommand request, CancellationToken cancellationToken)
        {
            var cafe = new Domain.Entities.Cafe
            {
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                Logo = request.Logo
            };

            await _repo.AddAsync(cafe);
            return cafe.CafeId;
        }
    }
}