

using Microsoft.Xna.Framework;

public class MainGame : Game
{
    private GraphicsDeviceManager _graphics;

    public MainGame()
    {
        // Add your game initialization code here
    }

    protected override void Initialize()
    {
        // Add your game initialization code here
        base.Initialize();

        _graphics = new GraphicsDeviceManager(this);
    }

    protected override void LoadContent()
    {
        // Add your game initialization code here
    }

    protected override void Update(GameTime gameTime)
    {
        // Add your game initialization code here
    }

    protected override void Draw(GameTime gameTime)
    {
        _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
    }
}