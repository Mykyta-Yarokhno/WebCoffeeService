using CoffeeMachineService;
using CoffeeMachineService.Models;
using System.Text.Json;

namespace WebCoffee.Service
{
    public class CoffeeMachine
    {
        private ICoffeeMachineControl _coffeeMachineInstance;

        public MachineState State { get; private set; } = MachineState.Off;
        public string Name { get; set; }
        public string NetworkLocation { get; set; }
        public string Id { get; set; }
        

        public CoffeeMachineAttributes Attributes { get;  } = new CoffeeMachineAttributes();
    
        public CoffeeMachine(string location, string name, string id)
        {
            Name = name;
            NetworkLocation = location;
            Id = id;
            _coffeeMachineInstance = new GenericCoffeeMachine(NetworkLocation);
        }

        public CoffeeMachine(CoffeeMachineConfig config)
            : this(BuildNetworkLocation(config), config.MachineName,config.MachineId)
        {
           
        }

        public static string BuildNetworkLocation(CoffeeMachineConfig machine)
        {
            return $"http://{machine.MachineIp}:{machine.Port}";
        }

        public static CoffeeMachine Create(CoffeeMachineConfig config)
        {
            return new CoffeeMachine(BuildNetworkLocation(config), config.MachineName, config.MachineId);
        }

        public void UpdateState()
        {
            var client = new HttpClient { BaseAddress = new Uri(NetworkLocation)};
            var getResult = client.GetAsync($"api/info").Result;

            var _currentCoffeeMachineInfo = (getResult.Content.ReadFromJsonAsync<CoffeeMachineInfo>()).Result;

            State = (_currentCoffeeMachineInfo?.State) ?? MachineState.Off;
        }

        public async Task UpdateStateAsync()
        {
            var client = new HttpClient { BaseAddress = new Uri(NetworkLocation) };
            var  getResult = await client.GetAsync($"api/info");

            var _currentCoffeeMachineInfo = await getResult.Content.ReadFromJsonAsync<CoffeeMachineInfo>();

            State = (_currentCoffeeMachineInfo?.State) ?? MachineState.Off;
        }

        public void TurnOn()
        {
            _coffeeMachineInstance.TurnOn();
        }

        public void TurnOff()
        {
            _coffeeMachineInstance.TurnOff();
        }

    }
}
