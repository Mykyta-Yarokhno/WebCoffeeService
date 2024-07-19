using CoffeeMachineService.Models;
using System.Text.Json.Serialization;

namespace WebCoffee.Service.Models
{
    public class CoffeeMachineInfo
    {
        public MachineState State { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NetworkLocation { get; set; }
        public string Id { get; set; }
    }
}
