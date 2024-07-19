using System.Collections.Concurrent;

namespace WebCoffee.Service
{
    public class CoffeeMachineAgentRunner
    {
        private ICoffeeOrdersProvider _ordersProvider;
        private ICoffeeMachineManager _machines;


        private List<CoffeeMachineAgent> _agents = new List<CoffeeMachineAgent>();

        public CoffeeMachineAgentRunner(ICoffeeOrdersProvider coffeeOrdersProvider, ICoffeeMachineManager machines)
        {
            _ordersProvider = coffeeOrdersProvider;
            _machines  =    machines;
        }

        public CoffeeMachineAgentRunner CreateAgents()
        {
            foreach (var machine in _machines)
            {
               _agents.Add(
                   new CoffeeMachineAgent(
                       new GenericCoffeeMachine(
                           machine.NetworkLocation, 
                           machine.Id
                           ), 
                       _ordersProvider)
                   );                              
            }

            return this;
        }

        public void RunAgents()
        {
            _agents.ForEach(x => x.Run());
        }
    }
}
