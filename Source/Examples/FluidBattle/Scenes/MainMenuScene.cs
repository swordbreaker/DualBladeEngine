﻿using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Entities;
using System.Collections.Generic;

namespace FluidBattle.Scenes;
public class MainMenuScene : GameScene
{
    public MainMenuScene(IGameContext context) : base(context)
    {
        context.Game.IsMouseVisible = true;
    }

    public override IEnumerable<ISystem> SetupSystems() =>
        [];

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        yield return CreateEntity(new MainMenuEntity(GameContext));
    }

    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
        GameContext.Game.IsMouseVisible = false;
    }
}
