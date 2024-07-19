using System.Text.Json.Serialization;

namespace WebCoffee.Service.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Awaiting,
        Preparing,
        Ready,
        Created
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Flags]
    public enum CoffeeOrderFilter
    {
        Preparing = 1,
        Awaiting = 2,
        Completed = 4,
        All = Preparing | Awaiting | Completed,
        NotCompleted = Preparing | Awaiting
    }

    public class CoffeeOrderInfo
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? User { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
        public CoffeeSettings CoffeeSettings { get; set; }
        public string? CoffeeId { get; set; }
        public CoffeeMachineInfo? CoffeeMachine { get; set; }


    }

}
