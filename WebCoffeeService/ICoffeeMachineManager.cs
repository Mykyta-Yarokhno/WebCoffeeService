namespace WebCoffee.Service
{
    public interface ICoffeeMachineManager : IEnumerable<CoffeeMachine>
    {
        CoffeeMachine? FindCoffeeMachineByName(string name);
        CoffeeMachine GetCoffeeMachine(string id);
        CoffeeMachine AddCoffeeMachine(CoffeeMachine coffeeMachine);       
    }
}
