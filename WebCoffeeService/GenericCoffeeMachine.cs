using CoffeeMachineService;
using CoffeeMachineService.Models;
using NLog;
using System.Text.Json;
using WebCoffee.Service.Common.Exceptions;

namespace WebCoffee.Service
{
    public class GenericCoffeeMachine : ICoffeeMachineControl
    {
        private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        private string _networkLocation;
        public string MachineId { get; }

        public GenericCoffeeMachine(string networkLocation)
        {
            _networkLocation = networkLocation;
        }

        public GenericCoffeeMachine(string networkLocation, string machineId) : this(networkLocation)
        {
            MachineId = machineId;
        }



        //public string MachineId => _coffeeMachine.Id;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drink"></param>
        /// <exception cref="CoffeeMachineException"> return status of coffee preparing</exception>
        public string PrepareDrink(CoffeeDrinkInfo drink)
        {
            _logger.Info($"Start preparing drink: [machineId={MachineId}]");
            _logger.Debug($"Drink info: [Type={drink.DrinkType},Cusize={drink.CupSize},Sugar={drink.Sugar}]");

            string? coffeeId = PostCoffeeMachineRequestAsync("api/coffee").Result;

            
            var client = new HttpClient { BaseAddress = new Uri(_networkLocation) };
            // TODO: Check coffeeId
            do
            {
                var getResult = client.GetAsync($"/api/coffee/{coffeeId}").Result;

                var coffeeStatus = getResult.Content.ReadAsStringAsync().Result;

                if(coffeeStatus == "OK")
                {
                    break;
                }
       
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();

            } while (true);

            _logger.Info($"Drink prepared: [drinkId={coffeeId},machineId={MachineId}]");

            return coffeeId;
        }

        public void TurnOn()
        {
            var coffeeInfo = PostCoffeeMachineRequestAsync<CoffeeMachineInfo>("api/coffee-machine/turn-on").Result;

            if (coffeeInfo == null)
            {
                throw new CoffeeMachineException(CoffeeMachineOperationStatus.Unknown, "No info received");
            }
                
            if( coffeeInfo.State == MachineState.On)
            {
                return;
            }

            throw new CoffeeMachineException(CoffeeMachineOperationStatus.Unavailable, "Coffee Machine Inactive");
        }

        public void TurnOff()
        {
            var coffeeInfo = PostCoffeeMachineRequestAsync<CoffeeMachineInfo>("api/coffee-machine/turn-off").Result;

            if (coffeeInfo == null)
            {
                throw new CoffeeMachineException(CoffeeMachineOperationStatus.Unknown, "No info received");
            }

            if (coffeeInfo.State == MachineState.Off)
            {
                return;
            }

            throw new CoffeeMachineException(CoffeeMachineOperationStatus.Unavailable, "Coffee Machine Inactive");
        }

        private async Task<T?> PostCoffeeMachineRequestAsync<T>(string method) 
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(_networkLocation) };

                var result = await client.PostAsync(method, null);

                return await result.Content.ReadFromJsonAsync<T>();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count > 0 && ex.InnerExceptions[0].GetType() == typeof(HttpRequestException))
                {
                    throw new CoffeeMachineException(CoffeeMachineOperationStatus.Unavailable, "", ex.InnerExceptions[0]);
                }

                throw new CoffeeMachineException(CoffeeMachineOperationStatus.Unknown, "Something wrong", ex);
            }
        }

        private async Task<string?> PostCoffeeMachineRequestAsync(string method)
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(_networkLocation) };

                var result = await client.PostAsync(method, null);

                return await result.Content.ReadAsStringAsync();
                
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count > 0 && ex.InnerExceptions[0].GetType() == typeof(HttpRequestException))
                {
                    throw new CoffeeMachineException(CoffeeMachineOperationStatus.Unavailable, "", ex.InnerExceptions[0]);
                }

                throw new CoffeeMachineException(CoffeeMachineOperationStatus.Unknown, "Something wrong", ex);
            }
        }
    }
}
