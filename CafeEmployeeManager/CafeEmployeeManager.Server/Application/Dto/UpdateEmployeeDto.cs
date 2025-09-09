namespace CafeEmployeeManager.Server.Application.Dto
{
        public record UpdateEmployeeDto(
            string Name,
            string EmailAddress,
            string PhoneNumber,
            string Gender,
            Guid? CafeId        // Optional re-assignment 
        );
    }
