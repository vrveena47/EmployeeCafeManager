using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEmployeeManager.Server.Domain.Entities
{
    public class Cafe
    {
        public Guid CafeId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte[]? Logo { get; set; } = null; // persisted in DB

        public string Location { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<EmployeeCafe> EmployeeCafe { get; set; } = new List<EmployeeCafe>();
    }
}