﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.Models;

namespace WebAPI.Services.Validation.DeveloperValidation
{
    public interface IDeveloperValidation
    {
        void Validate(Developer developer, ModelStateDictionary modelState);
    }
}
