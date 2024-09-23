using DualBlade.Core.Components;
using DualBlade.Core.Entities;

namespace ExampleGame.Components;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class CharacterComponent : IComponent
{
    public bool IsGrounded;
    public bool IsJumping;

    public IEntity Entity { get; init; }
}
