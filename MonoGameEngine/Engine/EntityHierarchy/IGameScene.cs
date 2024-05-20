using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Systems;
using System;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.Scenes;
public interface IGameScene : IDisposable
{
    IEntity Root { get; }

    IEnumerable<ISystem> Systems { get; }
}
