﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.Models;

namespace WebAPI.Services.Validation.UserValidation
{
    public interface IUserValidation
    {
        void Validate(User user,  ModelStateDictionary modelState);
    }
}
