using CafeEmployeeManager.Server.Application.Dto;
using CafeEmployeeManager.Server.Application.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace CafeEmployeeManager.Server.Application.Commands.Cafe
{
    public record UpdateCafeCommand(
        Guid CafeId,               // Immutable
        string Name,
        string Description,
        string Location,
        byte[]? Logo = null      // Optional
    ) : IRequest<CafeDto>;
    public class UpdateCafeCommandHandler : IRequestHandler<UpdateCafeCommand, CafeDto>
    {
        private readonly ICafeRepository _repo;
        public UpdateCafeCommandHandler(ICafeRepository repo) => _repo = repo;

        public async Task<CafeDto> Handle(UpdateCafeCommand request, CancellationToken cancellationToken)
        {
            var cafe = await _repo.GetByIdAsync(request.CafeId);
            if (cafe == null) return null!;

            cafe.Name = request.Name;
            cafe.Description = request.Description;
            cafe.Location = request.Location;
            cafe.Logo = request.Logo;

            await _repo.UpdateAsync(cafe);
            return new CafeDto
            {
                CafeId = cafe.CafeId,
                Name = cafe.Name,
                Description = cafe.Description,
                Location = cafe.Location,
                Logo = cafe.Logo != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(cafe.Logo)}" : null,
                Employees = cafe.EmployeeCafe.Count
            };
        }
    }
}