namespace WebCoffee.Service.Common.Exceptions
{
    public enum CoffeeMachineOperationStatus
    {
        Unavailable,
        Busy,
        InService,
        Off,
        Unknown
    }

    public class CoffeeMachineException: CoffeeServiceException
    {
        public CoffeeMachineException(CoffeeMachineOperationStatus status, string? message = null)
            :base (message)
        {
            Status = status;
        }

        public CoffeeMachineException(CoffeeMachineOperationStatus status, string? message , Exception innerException)
             : base(message, innerException)
        {
            Status = status;
        }

        public CoffeeMachineOperationStatus Status { get; init; }
        
    }
}
