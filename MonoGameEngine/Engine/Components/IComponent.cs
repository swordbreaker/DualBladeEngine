using MonoGameEngine.Engine.Entities;

namespace MonoGameEngine.Engine.Components;

public interface IComponent
{
    IEntity Entity { init; get; }
}
