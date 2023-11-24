using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.DeveloperValidation
{
    public class DeveloperValidation : IDeveloperValidation
    {
        public void Validate(Developer newDeveloper, DbSet<Developer> developers, ModelStateDictionary modelState)
        {
            if (IsAllLettersOrDigits(newDeveloper.Name) == false)
                modelState.AddModelError("NameLettersOrDigits", $"Developer name can contain only latin letters or digits");

            if (developers.Where(x => x.Name.ToLower() == newDeveloper.Name.ToLower()).Any())
                modelState.AddModelError("NameAlreadyExists", $"Developer with name {newDeveloper.Name} already exists");
        }

        public bool IsAllLettersOrDigits(string str)
        {
            foreach (char c in str)
            {
                if ((c >= 'a' && c <= 'z') == false && (c >= 'A' && c <= 'Z') == false && (c >= '0' && c <= '9') == false)
                    return false;
            }
            return true;
        }
    }
}
