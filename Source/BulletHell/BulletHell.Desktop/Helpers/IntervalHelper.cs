namespace BulletHell.Desktop.Helpers;

public struct IntervalHelper(float interval)
{
    private float lastUpdateTime;

    public bool Update(GameTime gameTime)
    {
        if (gameTime.TotalGameTime.TotalSeconds - lastUpdateTime >= interval)
        {
            lastUpdateTime = (float)gameTime.TotalGameTime.TotalSeconds;
            return true;
        }

        return false;
    }
}
