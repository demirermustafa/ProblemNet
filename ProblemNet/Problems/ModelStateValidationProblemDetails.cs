using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProblemNet.Problems
{
    public class ModelStateValidationProblemDetails : ProblemDetails
    {
        public ModelStateValidationProblemDetails()
        {
            Title = "One or more validation errors occurred.";
        }

        public ModelStateValidationProblemDetails(ModelStateDictionary modelState)
                : this()
        {
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));

            foreach (KeyValuePair<string, ModelStateEntry> keyValuePair in modelState)
            {
                string key = keyValuePair.Key;
                ModelErrorCollection errors = keyValuePair.Value.Errors;

                if (errors != null && errors.Any())
                {
                    var errorMessages = new List<string>();

                    for (var index = 0; index < errors.Count; ++index)
                    {
                        errorMessages.Add(GetErrorMessage(errors[index]));
                    }

                    Errors.Add(new ValidationProblem(key, errorMessages));
                }
            }
        }

        private string GetErrorMessage(ModelError error)
        {
            if (!string.IsNullOrEmpty(error.ErrorMessage))
                return error.ErrorMessage;

            return "The input was not valid";
        }

        public List<ValidationProblem> Errors { get; set; } = new List<ValidationProblem>();
    }
}
