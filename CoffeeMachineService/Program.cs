using CoffeeMachineService;
using CoffeeMachineService.Models;
using Microsoft.AspNetCore.Mvc;

string? currentCoffeeId = null;
Dictionary<string, string> coffeeStatus= new Dictionary<string, string>();
object locker = new();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration
            .AddJsonFile("coffeeguid.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<MachineStatus>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();



app.MapPost("api/coffee", () => 
{

    if (currentCoffeeId != null)
    {
        return "";
    }
    lock (locker)
    {
        if (currentCoffeeId != null)
        {
            return "";
        }
        currentCoffeeId = Guid.NewGuid().ToString();
        Task
            .Delay(20000)
            .ContinueWith((t) =>
            {
                coffeeStatus[currentCoffeeId] = "OK";
                currentCoffeeId = null;
            });

        return currentCoffeeId;
    }
}
);

app.MapGet("api/coffee/{coffeeid}", (string coffeeid) => 
{
    
    if(string.IsNullOrEmpty(coffeeid))
    {
        return /*Task.FromResult*/("INVALID ID");
    }

    if(coffeeid == currentCoffeeId)
    {
        return /*Task.FromResult*/("IN PROGRESS");
    }
    
    return /*Task.FromResult(*/coffeeStatus.ContainsKey(coffeeid) ? coffeeStatus[coffeeid] : "NOT FOUND";

});

app.MapGet("api/info", (IConfiguration configuration,MachineStatus machineStatus) =>
{
    return new CoffeeMachineInfo {  
        Name = $"CoffeeMachine_{configuration.GetValue<string>("MachineId")}", 
        State = machineStatus.GetCurrentMachineState(TimeSpan.FromSeconds(2))/*CurrentMachineState*/ 
    };
});

app.MapPost("api/coffee-machine/turn-on", (MachineStatus machineStatus) =>
{
    machineStatus.TurnOn();
    return new CoffeeMachineInfo { State = machineStatus.CurrentMachineState };
});

app.MapPost("api/coffee-machine/turn-off", (MachineStatus machineStatus) =>
{
    machineStatus.TurnOff();
    return new CoffeeMachineInfo { State = machineStatus.CurrentMachineState};
});

//app.MapControllers();

app.Run();
