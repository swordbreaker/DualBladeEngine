using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using System.Collections.Generic;

namespace DualBlade.Editor.Player.Yaml;
internal class EmptyScene(IGameContext context, EntityBuilder root) : GameScene(context)
{
    public override EntityBuilder Root => root;

    public override IEnumerable<ISystem> SetupSystems() => [];
    protected override IEnumerable<EntityBuilder> SetupEntities() => [root];
}
