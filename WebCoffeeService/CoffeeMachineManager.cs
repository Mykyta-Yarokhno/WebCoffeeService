using Microsoft.Extensions.Options;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using WebCoffee.Service.Common.Configuration;

namespace WebCoffee.Service
{
    public class CoffeeMachineFinder
    {
        public void Finder()
        {
            //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);

            //IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5000);
            //s.Bind(ipep);
            //s.SendAsync();
        }
    }

    public class CoffeeMachineManager : ICoffeeMachineManager
    {
        private readonly Dictionary<string, CoffeeMachine> _coffeeMachines = new();
        private readonly CoffeeMachineFinder _finder;
        //private readonly IConfiguration _configuration;
        private readonly IWritableOptions<CoffeeMachineConfigurations> _configuration;

        public CoffeeMachineManager(
            CoffeeMachineFinder finder,
            //IConfiguration configuration,
            IWritableOptions<CoffeeMachineConfigurations> configuration,
            IOptionsMonitor<CoffeeMachineConfigurations> options,
            CoffeeMachineConfigurations? config = null
            )
        {
            _finder = finder;
            _configuration = configuration;

            if (config != null)
            {
                //foreach(var machine in config.coffeeMachines)
                //{
                //    _coffeeMachines.Add(machine.Name, machine);
                //}
            }
#if DEBUG
            else
            {
                //var machinesConfig = configuration.GetSection(CoffeeMachineConfig.MachinesConfig).Get<CoffeeMachineConfigurations>();

                options.OnChange((config, x) => {
                    config.Machines.ForEach((machine) => {
                        if (!_coffeeMachines.ContainsKey(machine.MachineName))
                        {
                            _coffeeMachines.Add(machine.MachineName, CoffeeMachine.Create(machine));
                        }

                    });
                });

                var machinesConfig = options.CurrentValue;

                if (machinesConfig != null)
                {
                    foreach (var machine in machinesConfig.Machines)
                    {
                        _coffeeMachines.Add(machine.MachineName, CoffeeMachine.Create(machine));
                    }
                }

                #region Trash
                //var configRoot = (IConfigurationRoot)_configuration;

                //var newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions() { WriteIndented = true });

                //var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "coffeesettings.json");

                // File.WriteAllText(appSettingsPath, newJson);
                //_coffeeMachines.Add("Saeco", new CoffeeMachine("http://localhost:5000", "Saeco"));
                //_coffeeMachines.Add("Delongi",new CoffeeMachine("2.22.2.2", "Delongi"));
                #endregion
            }
#endif

        }

        public CoffeeMachine AddCoffeeMachine(CoffeeMachine coffeeMachine)
        {
            _coffeeMachines.Add(coffeeMachine.Name, coffeeMachine);

            _configuration.Update((change) =>
            {
                var url = new Uri(coffeeMachine.NetworkLocation);
                change.Machines.
                    Add(
                        new CoffeeMachineConfig
                        {
                            MachineName = coffeeMachine.Name,
                            MachineIp = url.Host,
                            Port = (uint)url.Port
                        });
            });

            return coffeeMachine;
        }
    

        public CoffeeMachine? FindCoffeeMachineByName(string name)
        {
            if (!_coffeeMachines.ContainsKey(name))
            {
                return null;
            }
                
            return _coffeeMachines[name];
        }

        public CoffeeMachine GetCoffeeMachine(string id)
        {
            return _coffeeMachines.Values.First((machine) => machine.Id == id);
        }

        public IEnumerator<CoffeeMachine> GetEnumerator()
        {
            return _coffeeMachines.Values.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return _coffeeMachines.Values.GetEnumerator();
        }
    }
}
