using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Utils;

namespace DualBlade.Core.Systems;

public abstract class ComponentSystem<TComponent1, TComponent2>(IGameContext gameContext) : BaseSystem(gameContext), IComponentSystem
    where TComponent1 : IComponent
    where TComponent2 : IComponent
{
    Memory<Type> IComponentSystem.CompTypes { get; } = new Type[] { typeof(TComponent1), typeof(TComponent2) }
        .Order(new SimpleTypeComparer())
        .ToArray();

    public override void Initialize() { }

    public override void Update(GameTime gameTime) { }
    public override void Draw(GameTime gameTime) { }
    public virtual void AfterDraw(GameTime gameTime) { }
    protected virtual void Update(ref TComponent1 component1, ref TComponent2 component2, ref IEntity entity, GameTime gameTime) { }
    protected virtual void Draw(TComponent1 component1, TComponent2 component2, IEntity entity, GameTime gameTime) { }
    protected virtual void OnAdded(ref IEntity entity, ref TComponent1 component, ref TComponent2 component2) { }
    protected virtual void OnDestroy(TComponent1 component, TComponent2 component2, IEntity entity) { }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    void IComponentSystem.OnAdded(IEntity entity, Span<IComponent> components, out IEntity outEntity, out Span<IComponent> outComponents)
    {
        GetComponentsTyped(components, out TComponent1 c1, out TComponent2 c2);

        OnAdded(ref entity, ref c1, ref c2);

        outComponents = new IComponent[] { c1, c2 };
        outEntity = entity;
    }

    void IComponentSystem.OnDestroy(IEntity entity, Span<IComponent> components)
    {
        GetComponentsTyped(components, out TComponent1 c1, out TComponent2 c2);
        OnDestroy(c1, c2, entity);
    }

    void IComponentSystem.Draw(IEntity entity, Span<IComponent> components, GameTime gameTime)
    {
        GetComponentsTyped(components, out TComponent1 c1, out TComponent2 c2);
        this.Draw(c1, c2, entity, gameTime);
    }

    void IComponentSystem.Update(IEntity entity, Span<IComponent> components, GameTime gameTime, out IEntity outEntity, out Span<IComponent> outComponents)
    {
        GetComponentsTyped(components, out TComponent1 c1, out TComponent2 c2);
        Update(ref c1, ref c2, ref entity, gameTime);

        outComponents = new IComponent[] { c1, c2 };
        outEntity = entity;
    }

    private void GetComponentsTyped(Span<IComponent> components, out TComponent1 component1, out TComponent2 component2)
    {
        component1 = default;
        component2 = default;

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
        }
    }
}
