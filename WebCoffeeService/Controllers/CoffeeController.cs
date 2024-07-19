using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebCoffee.Service.Controllers
{
    using System.Text.Json.Serialization;
    using WebCoffee.Service.Filters;
    using WebCoffee.Service.Models;


    [Route("api/coffee", Name = "coffee")]
    [ApiController]
    //[Authorize(Policy = "RequiredForCoffee")]
    public class CoffeeController : CoffeeBaseController
    {
        private readonly CoffeeService _coffeeService;


        public CoffeeController(CoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }

        
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoffeeOrderInfo))]
        //[ServiceFilter(typeof(LogResponceContent))]
        //public JsonResult MakeCoffee([FromBody]CoffeeSettings? coffeeSettings = null)
        //{
           
        //     return new JsonResult(_coffeeService.DoMakeCoffee(coffeeSettings, UserName, ""));
            
        //}


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoffeeOrderInfo))]
        [ServiceFilter(typeof(LogResponceContent))]
        public JsonResult MakeCoffee( string? machineId = null, [FromBody] CoffeeSettings? coffeeSettings = null)
        {

            return new JsonResult(_coffeeService.DoMakeCoffee(coffeeSettings, UserName, machineId));

        }

        
        [HttpGet]
        [Route("orders/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoffeeOrderInfo))]
        public CoffeeOrderInfo? GetOrderInfo(int orderId)
        {
            return _coffeeService.LookUpOrder(orderId);
        }

        [HttpGet]
        [Route("orders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CoffeeOrderInfo>))]
        public List<CoffeeOrderInfo> GetOrders(CoffeeOrderFilter OrdersType = CoffeeOrderFilter.All)
        {
            return _coffeeService.GetOrders(OrdersType);
        }

    }
}
