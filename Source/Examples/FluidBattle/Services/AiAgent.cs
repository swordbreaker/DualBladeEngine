using DualBlade.Core.Extensions;
using DualBlade.Core.Services;

namespace FluidBattle.Services;
public class AiAgent(IGameContext gameContext)
{
    private static readonly Random random = new();
    private readonly float _speed = 10f;
    private readonly IWorldToPixelConverter worldToPixelConverter = gameContext.GameEngine.WorldToPixelConverter;

    public Vector2 FluidTarget { get; private set; }
    private Vector2 _destination;

    public void Update(GameTime gameTime)
    {
        if (Distance(_destination, FluidTarget) < 0.2f)
        {
            var bound = worldToPixelConverter.WorldBounds;

            var x = random.NextFloat(bound.Left, bound.Right) - 1;
            var y = random.NextFloat(bound.Top, bound.Bottom) - 1;

            _destination = new Vector2(x, y);
        }

        var direction = Normalize(_destination - FluidTarget);
        FluidTarget += direction * _speed * gameTime.DeltaSeconds();
    }
}
