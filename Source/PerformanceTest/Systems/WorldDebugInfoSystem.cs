using DualBlade.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace DualBlade.Core.Systems;
internal class WorldDebugInfoSystem(IGameContext gameContext) : BaseSystem(gameContext)
{
    private SpriteFont spriteFont;
    private Color color = Color.Black;
    private Vector2 position = new(10, 30);

    public override void Initialize()
    {
        base.Initialize();
        spriteFont = GameContext.GameEngine.Load<SpriteFont>("DefaultFont");
    }

    public override void Draw(GameTime gameTime)
    {
        var displayText = $"Entities: {World.Entities.Count()}";
        displayText += $"\nSystems: {World.Systems.Count()}";
        displayText += $"\nComponents: {World.Components.Count()}";

        GameContext.GameEngine.SpriteBatch.Begin();
        GameContext.GameEngine.SpriteBatch!.DrawString(spriteFont, displayText, position, color);
        GameContext.GameEngine.SpriteBatch.End();
    }
}
