using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hintme.Models
{
    public class CoordinatesModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Metadata.ModelType != typeof(double) ? null : new CoordinatesModelBinder();
        }
    }
}