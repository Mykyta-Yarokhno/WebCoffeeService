using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text.Json;

namespace WebCoffee.Service.Filters
{
    public class LogResponceContent : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(Debugger.IsAttached)
            {
                Console.WriteLine(JsonSerializer.Serialize((context.Result  as JsonResult)?.Value));
            }
            


          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
           
        }
    }
}
