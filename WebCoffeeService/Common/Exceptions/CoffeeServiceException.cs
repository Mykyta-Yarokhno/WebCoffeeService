namespace WebCoffee.Service.Common.Exceptions
{
    public class CoffeeServiceException :Exception
    {
        public CoffeeServiceException(string? message, Exception? innerException = null)
            : base(message, innerException)
        {

        }

    }
}
