using System.Collections.Concurrent;
using WebCoffee.Service.Common.Exceptions;

namespace WebCoffee.Service
{
    public class CoffeeMachineAgent
    {
        private ICoffeeMachineControl _coffeeMachine;
        private ICoffeeOrdersProvider _ordersProvider;

        private Task _agentTask;

        public CoffeeMachineAgent(ICoffeeMachineControl coffeeMachine,ICoffeeOrdersProvider orders)
        {
            _coffeeMachine = coffeeMachine;
            _ordersProvider = orders;

            _agentTask = new Task(DoOrderProcessing);
        }

        private async void DoOrderProcessing()
        {
            DateTime lastProcessedOrder = DateTime.Now;
            bool machinePaused = false;

            do
            {
                CoffeeOrder? order = null;

                // Try take order
                if (!machinePaused)
                {
                    order = _ordersProvider.TakeOrder(_coffeeMachine.MachineId);
                }
                else
                {
                    // Stop taking orders until 5 minutes
                    await Task.Delay(TimeSpan.FromMinutes(5));

                    // Allow taking orders
                    machinePaused = false;
                    continue;
                }
                

                if(order != null)
                {
                    // Do prepare drink
                    try
                    {
                        order.Status = OrderStatus.Preparing;

                        var coffeeId  = _coffeeMachine.PrepareDrink(order.DrinkInfo);

                        order.CoffeeId = coffeeId;
                        order.CoffeeMachineId = _coffeeMachine.MachineId;
                        order.Status = OrderStatus.Ready;
                        lastProcessedOrder = DateTime.Now;

                        Console.WriteLine("Order received.");
                    }
                    catch(CoffeeMachineException ex)
                    {
                        // Stop taking orders until 5 minutes
                        machinePaused = true;

                        Console.WriteLine("failed coffee prepare");
                    }
                    
                }
                else
                {
                    Console.WriteLine("order is null");
                }
               
 
            } while (true);
        }

        public void Run()
        {
            _agentTask.Start();
        }

    }
}
