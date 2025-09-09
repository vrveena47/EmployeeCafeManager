using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEmployeeManager.Server.Application.Dto
{
        public record UpdateCafeDto(
            string Name,
            string Description,
            string Location,
            IFormFile? LogoFile
            );

    }
