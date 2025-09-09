
using CafeEmployeeManager.Server.Domain.Entities;

namespace CafeEmployeeManager.Server.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Employee entity.
    /// Handles all database operations related to employees.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Get all employees optionally filtered by Cafe name.
        /// </summary>
        /// <param name="cafeName">Optional cafe name filter</param>
        /// <returns>List of Employee entities</returns>
        Task<List<Employee>> GetAllAsync(string? cafeName);

        /// <summary>
        /// Get a single employee by Id
        /// </summary>
        /// <param name="employeeId">EmployeeId in format UIXXXXXXX</param>
        /// <returns>Employee entity if found, otherwise null</returns>
        Task<Employee?> GetByIdAsync(string employeeId);

        /// <summary>
        /// Add a new employee and optionally assign to a cafe
        /// </summary>
        /// <param name="employee">Employee entity to add</param>
        /// <returns></returns>
        Task AddAsync(Employee employee);

        /// <summary>
        /// Update an existing employee and optionally update cafe assignment
        /// </summary>
        /// <param name="employee">Employee entity with updated data</param>
        /// <returns></returns>
        Task UpdateAsync(Employee employee);

        /// <summary>
        /// Delete an employee by Id
        /// </summary>
        /// <param name="employeeId">EmployeeId in format UIXXXXXXX</param>
        /// <returns></returns>
        Task DeleteAsync(string employeeId);
        Task DeleteEmployeeCafeAsync(EmployeeCafe employeeCafe);
    }
}