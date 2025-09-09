namespace CafeEmployeeManager.Server.Domain.Entities
{
    public class EmployeeCafe
    {
        public string EmployeeId { get; set; } = null!;
        public Guid CafeId { get; set; }
        public DateTime StartDate { get; set; }

        public Employee Employee { get; set; } = null!;
        public Cafe Cafe { get; set; } = null!;
    }
}