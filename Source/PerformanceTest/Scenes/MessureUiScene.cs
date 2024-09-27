﻿using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PerformanceTest.Scenes;
public class MessureUiScene(IGameContext context) : GameScene(context)
{
    private readonly IGameEngine gameEngine = context.GameEngine;

    public override IEnumerable<ISystem> SetupSystems()
    {
        yield return CreateSystem<FpsDisplaySystem>();
        yield return CreateSystem<WorldDebugInfoSystem>();
    }

    protected override IEnumerable<INodeEntity> SetupEntities()
    {
        yield return new NodeEntity
        {
            Components = [new FpsDisplayComponent { Font = gameEngine.Load<SpriteFont>("DefaultFont") }]
        };
    }
}