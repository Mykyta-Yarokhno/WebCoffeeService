using System.Collections.Concurrent;

namespace WebCoffee.Service
{
    public class OrderIdGenerator
    {
        private volatile int _lastOrderId;

        public int CurrentId => _lastOrderId; 

        public OrderIdGenerator(int startId = 0)
        {
            _lastOrderId = startId;
        }

        public int GenerateId()
        {
            return Interlocked.Increment(ref _lastOrderId);
        }
    }

    public class CoffeeOrderService : ICoffeeOrderService
    {
        private OrderIdGenerator _idGenerator = new OrderIdGenerator();
        private ConcurrentDictionary<int, CoffeeOrder> _registeredOrders = new();

        public CoffeeOrder CreateOrder(CoffeeDrinkInfo coffee, string user, string coffeeMachineId)
        {
            var order =
                new CoffeeOrder
                {
                    DrinkInfo = coffee,
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.Created,
                    OrderID = _idGenerator.GenerateId(),
                    User = user,
                    CoffeeMachineId = coffeeMachineId
                };

            if(!_registeredOrders.TryAdd(order.OrderID, order))
            {
                throw new InvalidOperationException("Order already exist.");
            }

            return order;
        }

        public CoffeeOrder? FindOrder(int orderId)
        {
            if(!_registeredOrders.ContainsKey(orderId))
            {
                return null;
            }
            
            return _registeredOrders[orderId];
        }

        public IEnumerable<CoffeeOrder> GetOrders()
        {
            return _registeredOrders.Values;
        }
    }
}
