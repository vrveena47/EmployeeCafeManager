using CafeEmployeeManager.Server.Application.Interfaces;
using CafeEmployeeManager.Server.Domain.Entities;
using CafeEmployeeManager.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CafeEmployeeManager.Server.Infrastructure.Repositories
{ /// <summary>
  /// EF Core repository for Employees
  /// Handles all CRUD operations on Employees
  /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CafeEmployeeDbContext _dbContext;
        public EmployeeRepository(CafeEmployeeDbContext dbContext) => _dbContext = dbContext;

        /// <summary>
        /// Get all employees optionally filtered by cafe name, sorted by days worked descending
        /// </summary>
        public async Task<List<Employee>> GetAllAsync(string? cafeName)
        {
            var query = _dbContext.Employees
                .Include(e => e.EmployeeCafe)
                    .ThenInclude(ec => ec.Cafe)
                .AsQueryable();

            if (!string.IsNullOrEmpty(cafeName))
                query = query.Where(e => e.EmployeeCafe != null && e.EmployeeCafe.Cafe.Name == cafeName);

            return await query
                .OrderByDescending(e => e.EmployeeCafe != null ? EF.Functions.DateDiffDay(e.EmployeeCafe.StartDate, DateTime.UtcNow): 0)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(string employeeId) =>
            await _dbContext.Employees
                .Include(e => e.EmployeeCafe)
                    .ThenInclude(ec => ec.Cafe)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        public async Task AddAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string employeeId)
        {
            var employee = await _dbContext.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                // Remove EmployeeCafe relation first (if exists)
                if (employee.EmployeeCafe != null)
                {
                    _dbContext.EmployeeCafe.Remove(employee.EmployeeCafe);
                }
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
        }
        // Deletes an EmployeeCafe relation (used before changing Cafe assignment)
        public async Task DeleteEmployeeCafeAsync(EmployeeCafe employeeCafe)
        {
            _dbContext.EmployeeCafe.Remove(employeeCafe);
            await _dbContext.SaveChangesAsync();
        }
    }
}