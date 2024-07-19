namespace CoffeeMachineService.Models
{
    public class CoffeeMachineInfo
    {
        public string Name { get; set; } = "Undefined";
        public string? Description { get; set; }
        public MachineState State { get; set; }  = MachineState.On;
    }
}
