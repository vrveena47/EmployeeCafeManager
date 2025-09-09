namespace CafeEmployeeManager.Server.Domain.Entities
{
    public class Employee
    {
        public string EmployeeId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public EmployeeCafe? EmployeeCafe { get; set; }
    }
}