using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebCoffee.Service.Filters;
using WebCoffee.Service.Areas.Identity.Data;
using WebCoffee.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using WebCoffee.Service.Common.Auth;
using WebCoffee.Service.Common.Configuration;
using WebCoffee.Service.Common.Data;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebCoffeeDbContextConnection") ?? throw new InvalidOperationException("Connection string 'WebCoffeeDbContextConnection' not found.");


builder.Configuration
            .AddJsonFile("coffeesettings.json", optional: true, reloadOnChange: true);

builder.Services
    .AddDbContext<WebCoffeeDbContext>(options =>options.UseSqlite(connectionString));

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WebCoffeeDbContext>()
    ;


// Add services to the container.
builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Services.AddControllers();
builder.Services.ConfigureWritable<CoffeeMachineConfigurations>(builder.Configuration.GetSection(CoffeeMachineConfig.MachinesConfig), "coffeesettings.json");
//builder.Services.Configure<CoffeeMachineConfigurations>(builder.Configuration.GetSection(CoffeeMachineConfig.MachinesConfig));
builder.Services.AddSingleton<ICoffeeOrderService, CoffeeOrderService>();
builder.Services.AddSingleton<ICoffeeOrderProcessingService, CoffeeOrderProcessingService>();
builder.Services.AddSingleton<ICoffeeMachineManager, CoffeeMachineManager>();
builder.Services.AddSingleton<CoffeeMachineFinder>(); 
builder.Services.AddSingleton<CoffeeService>();
builder.Services.AddAutoMapper((serviceProvider, mapperConfiguration) =>
{
    mapperConfiguration.AddProfile(new MappingProfile(serviceProvider.GetRequiredService<ICoffeeMachineManager>()));
}, Array.Empty<Type>());

builder.Services.Configure<CoffeeMachineConfigurations>(c =>
{
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.11", " Saeco5"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco6"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco10"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.11.1", " Saeco2"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.11", " Saeco3"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco7"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco11"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco20"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco21"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco22"));
    //c.coffeeMachines.Add(new CoffeeMachine("1.11.1.1", " Saeco23"));
    //c.coffeeMachines.Add(new CoffeeMachine("2.22.2.2", " delongi"));
}
);


builder.Services.AddScoped<LogResponceContent>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddRazorPages();

//builder.Services
//    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddJwtBearer(options =>
//        {
//            options.SaveToken = true;
//            options.RequireHttpsMetadata = false;
//            options.TokenValidationParameters = new TokenValidationParameters()
//            {
//                ValidateIssuer = true,
//                ValidateAudience = false,
//                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
//            };
//        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequiredForCoffee",
         policy => policy.RequireAuthenticatedUser().RequireClaim("City", "Kiev"));
    options.AddPolicy("AdminRequired",
        policy => policy.RequireRole("admin"));

});

builder.Logging.AddConfiguration(builder.Configuration);


var app = builder.Build();

app.UseDefaultCoffeeRoles();

app.UseHttpLogging();
    
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();;

app.UseAuthorization();

app.UseStaticFiles();

app.MapRazorPages();

app.MapControllers();

app.Run();
