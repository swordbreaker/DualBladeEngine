using FluidBattle;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new GameServiceProvider();
serviceProvider.GetRequiredService<MainGame>().Run();