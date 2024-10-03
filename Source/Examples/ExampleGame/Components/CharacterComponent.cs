using DualBlade.Core.Components;

namespace ExampleGame.Components;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public partial struct CharacterComponent : IComponent
{
    public bool IsGrounded;
    public bool IsJumping;
}
