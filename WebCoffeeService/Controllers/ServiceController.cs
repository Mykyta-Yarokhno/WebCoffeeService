using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebCoffee.Service.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using WebCoffee.Service.Models;

    [Route("api/service", Name = "service")]
    [ApiController]
    [Authorize(Policy = "AdminRequired")]
    public class ServiceController : ControllerBase
    {
        private readonly CoffeeService _coffeeService;
        private readonly ICoffeeMachineManager _coffeeManager;
        private readonly IMapper _modelMapper;

        public ServiceController(CoffeeService coffeeService, ICoffeeMachineManager coffeeManager, IMapper modelMapper)
        {
            _coffeeService = coffeeService;
            _coffeeManager = coffeeManager;
            _modelMapper = modelMapper;
        }

        [HttpPost]
        [Route("machines")]
        public CoffeeMachineInfo RegisterMachine([Required] CoffeeMachineRegister coffeeMachine)
        {
            var coffeemachine = new CoffeeMachine(coffeeMachine.Location, coffeeMachine.MachineName, coffeeMachine.Id);

            return _modelMapper.Map<CoffeeMachine, CoffeeMachineInfo>(_coffeeManager.AddCoffeeMachine(coffeemachine));

        }

        [HttpGet]
        [Route("machines")]
        public async Task<IEnumerable<CoffeeMachineInfo>> GetMachines()
        {
            List<Task> tasks = new List<Task>();

            foreach (var coffeeMachine in _coffeeManager)
            {
                tasks.Add(coffeeMachine.UpdateStateAsync());
            }

            await Task.WhenAll(tasks.ToArray());

            return _coffeeManager.Select((machine) => _modelMapper.Map<CoffeeMachine, CoffeeMachineInfo>(machine));
        }

        [HttpGet]
        [Route("machines/{id}")]
        public IEnumerable<CoffeeMachineInfo> GetMachine(string id)
        {
            return _coffeeManager.Select((machine) => _modelMapper.Map<CoffeeMachine, CoffeeMachineInfo>(machine));
        }

        [HttpPost]
        [Route("machines/{id}/turn-on")]
        public void TurnOn(string id)
        {
            _coffeeService.GetCoffeeMachine(id)?.TurnOn();
        }

        [HttpPost]
        [Route("machines/{id}/turn-off")]
        public void TurnOff(string id)
        {
            _coffeeService.GetCoffeeMachine(id)?.TurnOff();
        }

        //[HttpGet]
        //public IReadOnlyCollection<CoffeeOrderInfo> GetOrders([FromQuery] OrderStatus? displayOrders)
        //{

        //    return _coffeeService.GetOrders(displayOrders, UserName);
        //} 

        //[HttpGet]
        //[Route("info")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoffeeMachineInfo))]
        //public CoffeeMachineInfo? GetInfo()
        //{
        //    return _coffeeService.GetCoffeeMachine()?.Info;
        //}


    }
}
