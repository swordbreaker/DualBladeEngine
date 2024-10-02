using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualBlade.Core.Systems;
public abstract class BaseComponentSystem(IGameContext gameContext) : BaseSystem(gameContext), IComponentSystem
{
    public abstract Memory<Type> CompTypes { get; }

    public override void Initialize() { }

    public override void Update(GameTime gameTime) { }
    public virtual void LateUpdate(GameTime gameTime) { }
    public override void Draw(GameTime gameTime) { }
    public virtual void LateDraw(GameTime gameTime) { }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public abstract void OnAdded(IEntity entity, Span<IComponent> components, out IEntity outEntity, out Span<IComponent> outComponents);

    public abstract void OnDestroy(IEntity entity, Span<IComponent> component);

    public abstract void Draw(IEntity entity, Span<IComponent> components, GameTime gameTime);

    public abstract void Update(IEntity entity, Span<IComponent> components, GameTime gameTime, out IEntity outEntity, out Span<IComponent> outComponents);
}
