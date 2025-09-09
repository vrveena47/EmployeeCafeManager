using CafeEmployeeManager.Server.Application.Dto;
using CafeEmployeeManager.Server.Application.Interfaces;
using MediatR;

namespace CafeEmployeeManager.Server.Application.Queries.Cafe
{
    public record GetCafeByIdQuery(Guid CafeId) : IRequest<CafeDto?>;

    public class GetCafeByIdQueryHandler : IRequestHandler<GetCafeByIdQuery, CafeDto?>
    {
        private readonly ICafeRepository _repo;
        public GetCafeByIdQueryHandler(ICafeRepository repo) => _repo = repo;

        public async Task<CafeDto?> Handle(GetCafeByIdQuery request, CancellationToken cancellationToken)
        {
            var cafe = await _repo.GetByIdAsync(request.CafeId);
            if (cafe == null) return null;

            return new CafeDto
            {
                CafeId = cafe.CafeId,
                Name = cafe.Name,
                Description = cafe.Description,
                Location = cafe.Location,
                Employees = cafe.EmployeeCafe.Count,
                Logo = cafe.Logo != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(cafe.Logo)}" : null
            };
        }
    }
}