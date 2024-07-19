using Microsoft.Extensions.Options;

namespace WebCoffee.Service.Common.Configuration
{
    public interface IWritableOptions<out T> : IOptions<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }

}
