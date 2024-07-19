namespace WebCoffee.Service
{
    public class CoffeeMachineConfig
    {
        public const string MachineNameUnknown = "Unknown Machine";
        public const string MachineNameDefault = MachineNameUnknown;

        public const string MachineIpDefault = "127.0.0.1";
        public const string MachinesConfig = "CoffeeMachineConfig";

        public string MachineIp { get; set; } = MachineIpDefault;

        public uint Port { get; set; }

        public string MachineName { get; set; } = MachineNameDefault;

        public string MachineId { get; set; } = "";
    }
}
