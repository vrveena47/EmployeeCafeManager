using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEmployeeManager.Server.Application.Dto
{// DTO for Cafe entity used in API responses
    public class CafeDto
    {
        public Guid CafeId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int Employees { get; set; }        // Number of employees in this cafe

        public IFormFile? LogoFile { get; set; }
        public string? Logo { get; set; } // base64 string
    }
}