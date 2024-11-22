using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Utils;

namespace DualBlade.Core.Systems;

public abstract class ComponentSystem<TComponent1, TComponent2, TComponent3>(IGameContext gameContext) : BaseComponentSystem(gameContext), IComponentSystem
    where TComponent1 : IComponent
    where TComponent2 : IComponent
    where TComponent3 : IComponent
{
    public override Memory<Type> CompTypes { get; } = new Type[] { typeof(TComponent1), typeof(TComponent2), typeof(TComponent3) }
        .Order(new SimpleTypeComparer())
        .ToArray();

    protected virtual void Update(ref TComponent1 component1, ref TComponent2 component2, ref TComponent3 component3, ref IEntity entity, GameTime gameTime) { }
    protected virtual void Draw(TComponent1 component1, TComponent2 component2, TComponent3 component3, IEntity entity, GameTime gameTime) { }
    protected virtual void OnAdded(ref IEntity entity, ref TComponent1 component, ref TComponent2 component2, ref TComponent3 component3) { }
    protected virtual void OnDestroy(TComponent1 component, TComponent2 component2, TComponent3 component3, IEntity entity) { }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    void IComponentSystem.OnAdded(IEntity entity, Span<IComponent> components, out IEntity outEntity, out Span<IComponent> outComponents)
    {
        GetComponentsTyped(components, out TComponent1 c1, out TComponent2 c2, out TComponent3 c3);

        OnAdded(ref entity, ref c1, ref c2, ref c3);

        outComponents = new IComponent[] { c1, c2, c3 };
        outEntity = entity;
    }

    void IComponentSystem.OnDestroy(IEntity entity, Span<IComponent> components)
    {
        GetComponentsTyped(components, out TComponent1 c1, out TComponent2 c2, out TComponent3 c3);
        OnDestroy(c1, c2, c3, entity);
    }

    void IComponentSystem.Draw(IEntity entity, Span<IComponent> components, GameTime gameTime)
    {
        GetComponentsTyped(components, out TComponent1 c1, out TComponent2 c2, out TComponent3 c3);
        this.Draw(c1, c2, c3, entity, gameTime);
    }

    void IComponentSystem.Update(IEntity entity, Span<IComponent> components, GameTime gameTime, out IEntity outEntity, out Span<IComponent> outComponents)
    {
        GetComponentsTyped(components, out TComponent1 c1, out TComponent2 c2, out TComponent3 c3);
        Update(ref c1, ref c2, ref c3, ref entity, gameTime);

        outComponents = new IComponent[] { c1, c2 };
        outEntity = entity;
    }

    private static void GetComponentsTyped(Span<IComponent> components, out TComponent1 component1, out TComponent2 component2, out TComponent3 component3)
    {
        component1 = default!;
        component2 = default!;
        component3 = default!;

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] is TComponent1 t1)
            {
                component1 = t1;
            }
            else if (components[i] is TComponent2 t2)
            {
                component2 = t2;
            }
            else if (components[i] is TComponent3 t3)
            {
                component3 = t3;
            }
        }
    }
}
