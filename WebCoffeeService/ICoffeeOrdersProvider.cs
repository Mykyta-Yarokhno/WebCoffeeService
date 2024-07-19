using System.Collections.Concurrent;

namespace WebCoffee.Service
{
    public interface ICoffeeOrdersProvider
    {
        CoffeeOrder TakeOrder(string coffeeMachineId);
    }
}
