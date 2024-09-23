using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade.Core.Factories;

public interface IEntityFactory
{
    T CreateEntity<T>() where T : IEntity;
}

internal class EntityFactory(IGameContext gameContext) : IEntityFactory
{
    public T CreateEntity<T>() where T : IEntity
    {
        var ctor = typeof(T).GetConstructors()
            .OrderBy(x => x.GetParameters().Length)
            .Last();

        if (ctor.GetParameters().Any(x => x.ParameterType == typeof(IGameContext)))
        {
            return (T)ActivatorUtilities.CreateInstance(gameContext.ServiceProvider, typeof(T), gameContext);
        }
        else
        {
            return (T)ActivatorUtilities.CreateInstance(gameContext.ServiceProvider, typeof(T));
        }
    }
}
