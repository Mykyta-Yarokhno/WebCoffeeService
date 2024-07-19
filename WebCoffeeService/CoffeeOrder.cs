
namespace WebCoffee.Service
{
    public enum OrderStatus
    {
        Awaiting,
        Preparing,
        Ready,
        Created
    }

    public class CoffeeOrder
    {    
        public string CoffeeId { get; set; }
        public string CoffeeMachineId { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? User { get; set; }
        public OrderStatus Status { get; set; }
        public CoffeeDrinkInfo DrinkInfo { get; set; } = CoffeeDrinkInfo.CreateDefaultDrink();

    }
}
