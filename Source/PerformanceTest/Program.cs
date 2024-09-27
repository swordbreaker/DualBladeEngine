using ExampleGame;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTest;

var serviceProvider = new GameServiceProvider();
serviceProvider.GetRequiredService<Game>().Run();