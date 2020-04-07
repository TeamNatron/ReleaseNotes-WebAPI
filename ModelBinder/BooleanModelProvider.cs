using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReleaseNotes_WebAPI.ModelBinder
{
    class BooleanModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(bool))
            {
                return new BooleanModelBinder();
            }
 
            return null;
        }
    }}