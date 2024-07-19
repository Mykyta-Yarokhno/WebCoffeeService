using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebCoffee.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCoffee.Service.Tests
{
    [TestClass()]
    public class CoffeeServiceTests
    {
        [TestMethod()]
        public void LookUpOrderTest()
        {
            //var service = new CoffeeService();
            //Assert.IsNotNull(service.LookUpOrder(1));
          
        }

        [TestMethod()]
        public void LookUpOrderIfCoffeeServiceEmpty()
        {
            //var service = new CoffeeService();
            //Assert.IsNull(service.LookUpOrder(1));
        }
    }
}