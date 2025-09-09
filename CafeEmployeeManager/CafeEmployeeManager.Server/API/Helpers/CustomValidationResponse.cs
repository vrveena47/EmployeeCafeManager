using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace CafeEmployeeManager.Server.API.NewFolder
{
    public static class CustomValidationResponse
    {
        public static IActionResult GenerateResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var result = new
            {
                message = "Validation failed for one or more fields.",
                errors
            };

            return new BadRequestObjectResult(result);
        }
    }
}