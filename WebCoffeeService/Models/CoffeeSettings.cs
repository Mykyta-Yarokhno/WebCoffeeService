using System.Text.Json.Serialization;

namespace WebCoffee.Service.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CupSize
    {
        S,
        M,
        L
    
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CoffeeType
    {
        Americano,
        Espresso
    }

    public class CoffeeSettings
    {
        public CupSize Size { get; set; } = CupSize.M;
        public CoffeeType Type { get; set; } = CoffeeType.Espresso;
        public uint Sugar { get; set; } = 0;

    }
}
