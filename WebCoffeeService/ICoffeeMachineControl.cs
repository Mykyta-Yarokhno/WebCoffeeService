namespace WebCoffee.Service
{
    public interface ICoffeeMachineControl
    {
        string MachineId { get; }

        string PrepareDrink(CoffeeDrinkInfo drink);

        void TurnOn();

        void TurnOff();
    }
}
