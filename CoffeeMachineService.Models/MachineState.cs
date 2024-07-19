using System.Text.Json.Serialization;

namespace CoffeeMachineService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MachineState
    {
        TurnsOn, 
        On,
        Off,
        TurnsOff,
        Unknown
    }
}
