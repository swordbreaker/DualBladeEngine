using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Entities;

namespace ExampleGame.Components;
public class CharacterComponent : IComponent
{
    public bool IsGrounded;

    public IEntity Entity { get; init; }
}
