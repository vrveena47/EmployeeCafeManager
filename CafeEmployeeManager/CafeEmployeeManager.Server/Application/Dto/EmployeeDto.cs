namespace CafeEmployeeManager.Server.Application.Dto
{ // DTO for Employee entity used in API responses
    public class EmployeeDto
    {
        public string EmployeeId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string? Cafe { get; set; } = null;
        public Guid? CafeId { get; set; } = null;  // Cafe name or empty if unassigned
        public int DaysWorked { get; set; }               // Calculated from StartDate
    }
}