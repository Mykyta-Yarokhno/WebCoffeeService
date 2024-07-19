using System.Collections.Concurrent;

namespace WebCoffee.Service
{
    public interface IOrdersQueue
    {
        void AddOrder(CoffeeOrder order);
        void RegisterCoffeeMachine(string coffeeMachineId);
    }
}
