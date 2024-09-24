using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using System.Collections.Generic;

namespace DualBlade.Editor.Player.Yaml;
internal class EmptyScene : GameScene
{
    private IEntity _root;

    public EmptyScene(IGameContext context) : base(context)
    {
    }

    public override IEntity Root => this._root;

    public void SetRoot(IEntity root) => this._root = root;

    public override IEnumerable<ISystem> SetupSystems() => [];
    protected override IEnumerable<IEntity> SetupEntities() => [];
}
