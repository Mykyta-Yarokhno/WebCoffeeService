using System.Collections.Concurrent;

namespace WebCoffee.Service
{
    public class CoffeeOrdersBlockingQueue : ICoffeeOrdersProvider, IOrdersQueue
    {
        private Dictionary<string, BlockingCollection<CoffeeOrder>>? _awaitingOrders = new();

        public const string QueueAny = "Any";

        public CoffeeOrdersBlockingQueue()
        {
            _awaitingOrders.Add(QueueAny, new BlockingCollection<CoffeeOrder>());
        }

        public void AddOrder(CoffeeOrder order)
        {
            var queueId = order.CoffeeMachineId;

            if (string.IsNullOrEmpty(order.CoffeeMachineId)) queueId = QueueAny;

            _awaitingOrders[queueId].Add(order);

            //if(string.IsNullOrEmpty(order.CoffeeMachineId))
            //{
            //    _awaitingOrders[QueueAny].Add(order);
            //}
            //else
            //{
            //    _awaitingOrders[order.CoffeeMachineId].Add(order);

            //}
           
        }

        public void RegisterCoffeeMachine(string coffeeMachineId)
        {
            if (_awaitingOrders.ContainsKey(coffeeMachineId))
                return;

            _awaitingOrders.Add(coffeeMachineId, new BlockingCollection<CoffeeOrder>());
        }

        public CoffeeOrder? TakeOrder(string coffeeMachineId)
        {
            var index = BlockingCollection<CoffeeOrder>
                .TakeFromAny(new[] { _awaitingOrders[coffeeMachineId], _awaitingOrders[QueueAny] }, out CoffeeOrder? coffeeOrder);

            return coffeeOrder;
        }
    }
}
