using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReleaseNotes_WebAPI.Domain.ModelBinder
{
    public interface IBooleanModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext);
    }
}