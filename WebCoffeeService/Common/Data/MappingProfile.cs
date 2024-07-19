using AutoMapper;
using WebCoffee.Service.Models;

namespace WebCoffee.Service.Common.Data
{
    public class MappingProfile: Profile
    {
        public MappingProfile(ICoffeeMachineManager coffeeMachines)
        {

            CreateMap<CoffeeMachine, CoffeeMachineInfo>().ReverseMap();

            CreateMap<char, Models.CupSize>().ConvertUsing((src, dest) => src switch
            {
                (char)CupSize.M => Models.CupSize.M,
                (char)CupSize.L => Models.CupSize.L,
                _ => Models.CupSize.S,
            });

            CreateMap<string, CoffeeType>().ConvertUsing((src, dest) => src switch
            {
                CoffeeDrinkTypes.Espresso => CoffeeType.Espresso,
                CoffeeDrinkTypes.Americano=> CoffeeType.Americano,
                _=> CoffeeType.Americano,

            });

            CreateMap<CoffeeDrinkInfo, CoffeeSettings>();
               

            CreateMap<CoffeeOrder, CoffeeOrderInfo>()
                .ForMember(dest =>
                    dest.CoffeeMachine,
                    opt => opt.MapFrom(Src =>
                        Src.CoffeeMachineId != null ? coffeeMachines.GetCoffeeMachine(Src.CoffeeMachineId) : null))
                .ForMember(dest => dest.CoffeeSettings, opt => opt.MapFrom(Src => Src.DrinkInfo));
        }
    }
}
