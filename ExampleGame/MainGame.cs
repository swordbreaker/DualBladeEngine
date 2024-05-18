﻿using ExampleGame.Entities;
using ExampleGame.Systems;
using Microsoft.Xna.Framework;
using MonoGameEngine;
using MonoGameEngine.Engine.Worlds;
using Myra.Graphics2D.UI;

namespace ExampleGame;

public class MainGame : BaseGame
{
    public MainGame(WorldFactory worldFactory, IGameEngineFactory gameEngineFactory) : base(worldFactory, gameEngineFactory)
    {
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        base.Initialize();

        var (w, h) = GameEngine.GameSize;

        GameWorld.AddEntity(new BallEntity(GameEngine)
        {
            World = GameWorld
        });
        GameWorld.AddEntity(new GroundEntity(GameEngine)
        {
            World = GameWorld
        });

        var left = new GroundEntity(GameEngine)
        {
            World = GameWorld,
            Color = Color.Black,
            Position = new Vector2(0, h / 2f),
            Size = new Vector2(10, h)
        };

        var right = new GroundEntity(GameEngine)
        {
            World = GameWorld,
            Color = Color.Black,
            Position = new Vector2(w, h / 2f),
            Size = new Vector2(10, h)
        };
        
        AddEntity(left);
        AddEntity(right);

        GameWorld.AddSystem(new BallSystem(GameEngine.CameraService) { World = GameWorld });
        GameWorld.AddSystem<SpawnSystem>();
    }

    protected override void LoadContent()
    {
        string data = File.ReadAllText("Content/Menu.xmmp");
        var project = Project.LoadFromXml(data);
        Desktop.Root = project.Root;
    }
}
