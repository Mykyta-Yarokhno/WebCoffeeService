using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebCoffee.Service.Models
{

    public class CoffeeLogin
    {

        //[Required(ErrorMessage = "User Name is required")]
        [DefaultValue ("coffee.admin@gmail.com")]
        public string? Username { get; set; } 

        //[Required(ErrorMessage = "Password is required")]
        [DefaultValue("_Aa123456")]
        public string? Password { get; set; } 


    }
}
