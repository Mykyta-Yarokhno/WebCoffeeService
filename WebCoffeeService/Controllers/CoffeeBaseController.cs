using Microsoft.AspNetCore.Mvc;

namespace WebCoffee.Service.Controllers
{
    public abstract class CoffeeBaseController : ControllerBase
    {
        protected string UserName => User?.Identity?.Name ?? "";
    }
}
