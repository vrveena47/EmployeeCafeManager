using CafeEmployeeManager.Server.Domain.Entities;

namespace CafeEmployeeManager.Server.Application.Interfaces
{
    public interface ICafeRepository
    {
        Task<List<Cafe>> GetAllAsync(string? location);
        Task<Cafe?> GetByIdAsync(Guid cafeId);
        Task AddAsync(Cafe cafe);
        Task UpdateAsync(Cafe cafe);
        Task DeleteAsync(Guid cafeId);
    }
}