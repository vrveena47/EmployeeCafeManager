using CafeEmployeeManager.Server.Application.Interfaces;
using CafeEmployeeManager.Server.Domain.Entities;
using CafeEmployeeManager.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CafeEmployeeManager.Server.Infrastructure.Repositories
{/// <summary>
 /// EF Core repository for Cafes
 /// Handles all CRUD operations on Cafes
 /// </summary>
    public class CafeRepository : ICafeRepository
    {
        private readonly CafeEmployeeDbContext _dbContext;
        public CafeRepository(CafeEmployeeDbContext dbContext) => _dbContext = dbContext;

        /// <summary>
        /// Get all cafes, optionally filter by location, sorted by employee count descending
        /// </summary>
        public async Task<List<Cafe>> GetAllAsync(string? location)
        {
            var query = _dbContext.Cafes.Include(c => c.EmployeeCafe).AsQueryable();
            if (!string.IsNullOrEmpty(location))
                query = query.Where(c => c.Location == location);
            return await query
                .OrderByDescending(c => c.EmployeeCafe.Count)
                .ToListAsync();
        }

        public async Task<Cafe?> GetByIdAsync(Guid cafeId) =>
            await _dbContext.Cafes
                .Include(c => c.EmployeeCafe)
                .FirstOrDefaultAsync(c => c.CafeId == cafeId);

        public async Task AddAsync(Cafe cafe)
        {
            _dbContext.Cafes.Add(cafe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cafe cafe)
        {
            _dbContext.Cafes.Update(cafe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid cafeId)
        {
            var cafe = await _dbContext.Cafes.FindAsync(cafeId);
            if (cafe != null)
            {
                _dbContext.Cafes.Remove(cafe);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}