using CafeEmployeeManager.Server.Application.Dto;
using CafeEmployeeManager.Server.Application.Interfaces;
using MediatR;

namespace CafeEmployeeManager.Server.Application.Queries.Cafe
{
    public record GetAllCafesQuery(string? Location) : IRequest<List<CafeDto>>;

    public class GetAllCafesQueryHandler : IRequestHandler<GetAllCafesQuery, List<CafeDto>>
    {
        private readonly ICafeRepository _repo;
        public GetAllCafesQueryHandler(ICafeRepository repo) => _repo = repo;

        public async Task<List<CafeDto>> Handle(GetAllCafesQuery request, CancellationToken cancellationToken)
        {
            var cafes = await _repo.GetAllAsync(request.Location);
            return cafes.Select(c => new CafeDto
            {
                CafeId = c.CafeId,
                Name = c.Name,
                Description = c.Description,
                Location = c.Location,
                Employees = c.EmployeeCafe.Count,
                Logo = c.Logo != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(c.Logo)}" : null
            }).ToList();
        }
    }
}