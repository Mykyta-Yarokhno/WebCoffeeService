using System.Collections.Concurrent;
using System.Linq;
using WebCoffee.Service.Common.Exceptions;

namespace WebCoffee.Service
{
    public class CoffeeOrderProcessingService : ICoffeeOrderProcessingService
    {

        private CoffeeOrdersBlockingQueue _awaitingOrders = new();

        private CoffeeMachineAgentRunner _agents;

        private ICoffeeMachineManager _coffeeMachines;

        public CoffeeOrderProcessingService(ICoffeeMachineManager coffeeMachines)
        {
            _coffeeMachines = coffeeMachines;

            foreach(var coffeeMachine in _coffeeMachines)
            {
                // Register new queue for coffee machine
                _awaitingOrders.RegisterCoffeeMachine(coffeeMachine.Id);
            }

            _agents = new CoffeeMachineAgentRunner(_awaitingOrders, coffeeMachines);

            _agents.CreateAgents().RunAgents();
        }

        public void ProcessOrder(CoffeeOrder order)
        {
            RegisterOrder(order);
        }

        private void RegisterOrder(CoffeeOrder order)
        {
            order.Status = OrderStatus.Awaiting;

            if (!string.IsNullOrEmpty(order.CoffeeMachineId))
            {
                // Check if coffee machine with such id exists
                if (_coffeeMachines.FirstOrDefault<CoffeeMachine>((x) => x.Id == order.CoffeeMachineId) == null)
                {
                    throw new CoffeeServiceException("Coffee machine with such id doesn't exist");
                }

            }

            _awaitingOrders.AddOrder(order);
        }
    }
}
