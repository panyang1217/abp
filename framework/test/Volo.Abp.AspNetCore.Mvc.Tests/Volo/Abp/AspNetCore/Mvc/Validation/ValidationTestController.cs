﻿using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    [Route("api/validation-test")]
    public class ValidationTestController : AbpController
    {
        [HttpGet]
        [Route("object-result-action")]
        public Task<string> ObjectResultAction(ValidationTest1Model model)
        {
            ModelState.IsValid.ShouldBeTrue(); //AbpValidationFilter throws exception otherwise
            return Task.FromResult(model.Value1);
        }

        [HttpGet]
        [Route("action-result-action")]
        public IActionResult ActionResultAction(ValidationTest1Model model)
        {
            return Content("ModelState.IsValid: " + ModelState.IsValid.ToString().ToLowerInvariant());
        }

        public class ValidationTest1Model
        {
            [Required]
            [MinLength(2)]
            public string Value1 { get; set; }
        }
    }
}