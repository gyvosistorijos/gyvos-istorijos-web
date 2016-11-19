using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hintme.Models
{
    public class CoordinatesModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None || string.IsNullOrEmpty(valueProviderResult.FirstValue))
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
            bindingContext.Result = ModelBindingResult.Success(double.Parse(valueProviderResult.FirstValue, CultureInfo.InvariantCulture));

            return Task.CompletedTask;
        }
    }
}