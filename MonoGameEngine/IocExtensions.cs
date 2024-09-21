﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonoGameEngine.Engine.Physics;
using MonoGameEngine.Engine.Rendering;
using MonoGameEngine.Engine.Services;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine;

public static class IocExtensions
{
    public static IHostBuilder AddGameEngine(this IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureServices(context =>
        {
            context.AddSingleton<InputManager>();
            context.AddSingleton<IGameEngineFactory, GameEngineFactory>();
            context.AddSingleton<IWorldFactory, WorldFactory>();
            context.AddSingleton<IPhysicsManager, PhysicsManager>();
            context.AddSingleton<ISystemFactory, SystemFactory>();
            context.AddSingleton<ISpriteFactory, SpriteFactory>();
            context.AddSingleton<ICameraServiceFactory, CameraServiceFactory>();
            context.AddSingleton<IWorldToPixelConverterFactory, WorldToPixelConverterFactory>();
            context.AddSingleton<ISceneManagerFactory, SceneManagerFactory>();
            context.AddSingleton<IJobQueue, JobQueue>();
        });
}
