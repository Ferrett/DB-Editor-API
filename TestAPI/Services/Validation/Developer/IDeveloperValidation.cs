using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.DeveloperValidation
{
    public interface IDeveloperValidation
    {
        void Validate(Developer newDeveloper, List<Developer> developers, ModelStateDictionary modelState);
    }
}
