using System.Collections.Concurrent;

namespace WebCoffee.Service
{
    using AutoMapper;
    using System.Security.Claims;
    using WebCoffee.Service.Models;
    using NLog;

    public class CoffeeService
    {
        private ICoffeeOrderService _orderService;
        private ICoffeeOrderProcessingService _processingService;
        private readonly IMapper _modelMapper;
        private ICoffeeMachineManager _machineManager;
        private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public CoffeeService(
            ICoffeeOrderService orders
            , ICoffeeOrderProcessingService processingService
            , IMapper modelMapper
            , ICoffeeMachineManager machineManager
            )
        {
            _orderService = orders;
            _processingService = processingService;
            _modelMapper = modelMapper;
            _machineManager = machineManager;
        }

        public virtual CoffeeOrder DoMakeCoffee(CoffeeSettings? coffeeSettings,  string currentUser, string coffeeMachineId)
        {
            _logger.Info($"Do Make Coffee: [user={currentUser}, machineId={coffeeMachineId ?? "Undefined"}].");
            CoffeeDrinkInfo drink;

            if (coffeeSettings == null)
            {
                drink = CoffeeDrinkInfo.CreateDefaultDrink();
                   
            }
            else
            {
                drink =
                    new CoffeeDrinkInfo
                    {
                        CupSize = coffeeSettings.Size.ToString()[0],
                        DrinkType = coffeeSettings.Type.ToString(),
                        Sugar = coffeeSettings.Sugar
                    };

            }
            var order = _orderService.CreateOrder(drink, currentUser, coffeeMachineId);

            _logger.Info($"Order processing: [orderId={order.OrderID}]");
            _processingService.ProcessOrder(order);

            return order;
        }


        public CoffeeMachine? GetCoffeeMachine(string id)
        {
            return _machineManager.FirstOrDefault(x =>  x.Id == id);
        }

        public CoffeeOrderInfo? LookUpOrder(int orderId)
        {
            var order = _orderService.FindOrder(orderId);
            if(order == null)
            {
                return null;
            }

            return  _modelMapper.Map<CoffeeOrder, CoffeeOrderInfo>(order);
        }



        public List<CoffeeOrderInfo> GetOrders(CoffeeOrderFilter displayOrders)
        {
            var res = _orderService.GetOrders();
            
            res = res.Where((x) => {
                bool filterExp = false;
                if (displayOrders.HasFlag(CoffeeOrderFilter.Preparing)) filterExp =  x.Status == OrderStatus.Preparing;
                if (!filterExp && displayOrders.HasFlag(CoffeeOrderFilter.Completed) ) filterExp =  x.Status == OrderStatus.Ready;
                if (!filterExp &&  displayOrders.HasFlag(CoffeeOrderFilter.Awaiting)) filterExp = x.Status == OrderStatus.Awaiting || x.Status == OrderStatus.Created;
                return filterExp;
            });

            return _modelMapper.Map<IEnumerable<CoffeeOrder>, List<CoffeeOrderInfo>>(res);
        }
    }
}
