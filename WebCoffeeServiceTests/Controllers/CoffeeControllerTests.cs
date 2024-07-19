using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebCoffee.Service.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using WebCoffee.Service.Models;

namespace WebCoffee.Service.Controllers.Tests
{

    [TestClass()]
    public class CoffeeControllerTests
    {
        Mock<CoffeeService> mockCS = new ();

        public CoffeeControllerTests()
        {
            //mockCS
            //   .Setup(q => q.DoMakeCoffee(null, It.IsAny<string>()))
            //   .Returns(new CoffeeOrderInfo { OrderID = 2 });
            //mockCS
            //   .Setup(q => q.DoMakeCoffee(It.IsNotNull<CoffeeSettings>(), It.IsAny<string>()))
            //   .Returns(new CoffeeOrderInfo { OrderID = 1 });
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("ClassCleanup");
        }

        [TestMethod()]
        public void MakeCoffeeTest()
        {
            var newCofee = (new CoffeeController(mockCS.Object)).MakeCoffee();

            Assert.IsNotNull(newCofee.Value);
            Assert.IsInstanceOfType(newCofee.Value, typeof(CoffeeOrderInfo));
            Assert.AreEqual(1, (newCofee.Value as CoffeeOrderInfo)?.OrderID);
        }

        [TestMethod()]
        public void MakeCoffeeTest2()
        {
            var newCofee = (new CoffeeController(mockCS.Object)).MakeCoffee(null);

            Assert.IsNotNull(newCofee.Value);
            Assert.IsInstanceOfType(newCofee.Value, typeof(CoffeeOrderInfo));
            Assert.AreEqual(2, (newCofee.Value as CoffeeOrderInfo)?.OrderID);
        }
    }
}