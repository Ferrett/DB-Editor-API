using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Services.Validation.DeveloperValidation
{
    public class DeveloperValidation : IDeveloperValidation
    {
        private readonly ApplicationDbContext dbcontext;
        public DeveloperValidation(ApplicationDbContext context)
        {
            dbcontext = context;
        }
        public void Validate(Developer developer, ModelStateDictionary modelState)
        {
            if (!dbcontext.Developer.Any(x => x.ID == developer.ID) && dbcontext.Developer.Any(x => x.Name.ToLower() == developer.Name.ToLower()))
                modelState.AddModelError("NameAlreadyExists", $"Developer with name \"{developer.Name}\" already exists");

            if (IsAllLettersOrDigits(developer.Name) == false)
                modelState.AddModelError("NameLettersOrDigits", "Developer name can contain only latin letters or digits");
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
