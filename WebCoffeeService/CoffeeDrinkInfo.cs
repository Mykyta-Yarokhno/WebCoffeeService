namespace WebCoffee.Service
{
    public static class CoffeeDrinkTypes
    {
        public const string Espresso = "Espresso";

        public const string DrinkUnknown = "Unknown";

        public const string Americano = "Americano";
    }

    public  enum CupSize
    {
        S = 'S',
        M = 'M',
        L = 'L'
    }


    public class CoffeeDrinkInfo
    {
     
        public string DrinkType { get; set; } = CoffeeDrinkTypes.DrinkUnknown;

        public char CupSize { get; set; }

        public uint Sugar { get; set; } = 0;

        public static CoffeeDrinkInfo CreateDefaultDrink()
        {
            return new CoffeeDrinkInfo { DrinkType = DefaultDrinkType, CupSize = 'M', Sugar = 1};
        }

        public static string DefaultDrinkType => CoffeeDrinkTypes.Americano;

    }
}
