#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CoffeeMachineService/CoffeeMachineService.csproj", "CoffeeMachineService/"]
RUN dotnet restore "CoffeeMachineService/CoffeeMachineService.csproj"
COPY . .
WORKDIR "/src/CoffeeMachineService"
RUN dotnet build "CoffeeMachineService.csproj" -c Release -o /app/build

FROM build AS publish
<<<<<<< HEAD
RUN dotnet publish "CoffeeMachineService.csproj" -c Release -o /app/publish /p:UseAppHost=false
=======
RUN dotnet publish "CoffeeMachineService.csproj" -c Release -o /app/publish
>>>>>>> test_feature_2

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeMachineService.dll"]