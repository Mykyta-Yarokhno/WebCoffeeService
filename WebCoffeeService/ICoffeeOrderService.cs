namespace WebCoffee.Service
{
    public interface ICoffeeOrderService
    {
        CoffeeOrder CreateOrder(CoffeeDrinkInfo coffee, string user, string coffeeMachineId);

        CoffeeOrder? FindOrder(int orderId);

        IEnumerable<CoffeeOrder> GetOrders();
    }
}
