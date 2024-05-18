namespace MonoGameEngine.Engine.Worlds;

public interface IWorldFactory
{
    IWorld Create(IGameEngine engine);
}