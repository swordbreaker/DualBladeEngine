using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Jab;
using Microsoft.Xna.Framework.Content;

namespace DualBlade.Core;

[ServiceProviderModule]
[Singleton<IInputManager, InputManager>]
[Singleton<IGameEngineFactory, GameEngineFactory>]
[Singleton<IWorldFactory, WorldFactory>]
[Singleton<ISystemFactory, SystemFactory>]
[Singleton<ISceneManagerFactory, SceneManagerFactory>]
[Singleton<IJobQueue, JobQueue>]
[Singleton<IGameContext, GameContext>]
[Singleton<IGameCreationContext, GameCreationContext>]
[Singleton<ICameraServiceFactory, CameraServiceFactory>]
[Singleton<IWorldToPixelConverterFactory, WorldToPixelConverterFactory>]
[Singleton<ContentManager>]
[Transient<InputSystem>]
[Transient<FpsDisplaySystem>]
public interface ICoreServiceModule
{
}
