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
        }
    }
}
