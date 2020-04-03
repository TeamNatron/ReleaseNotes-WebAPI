using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReleaseNotes_WebAPI.ModelBinder
{
    class BooleanModelBinder : IModelBinder
    {
        /**
         * Makes sure that every boolean query parameter gets interpreted as false if they're not
         * included, and true if they truly are included. 
         */
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (result == ValueProviderResult.None)
            {
                // Parameter is missing, interpret as false
                bindingContext.Result = ModelBindingResult.Success(false);
            }
            else
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, result);
                var rawValue = result.FirstValue;
                if (string.IsNullOrEmpty(rawValue))
                {
                    // Value is empty, interpret as true
                    bindingContext.Result = ModelBindingResult.Success(true);
                }
                else if (bool.TryParse(rawValue, out var boolValue))
                {
                    // Value is a valid boolean, use that value
                    bindingContext.Result = ModelBindingResult.Success(boolValue);
                }
                else
                {
                    // Value is something else, fail
                    bindingContext.ModelState.TryAddModelError(
                        bindingContext.ModelName,
                        "Value must be false, true, or empty.");
                }
            }
 
            return Task.CompletedTask;
        }
    }
}