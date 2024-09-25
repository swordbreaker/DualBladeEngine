using DualBlade._2D.Rendering.Systems;
using DualBlade.Analyzer;
using DualBlade.Core;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Editor.Player.Entities;
using DualBlade.Editor.Player.Services;
using DualBlade.Editor.Player.Systems;
using DualBlade.Editor.Player.Yaml;
using DualBlade.MyraUi.Systems;
using Editor.Systems;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using ToolsUtilities;

namespace Editor;
public class EditorGame : BaseGame
{
    private SceneGenerator sceneGenerator;
    private SceneRoot sceneRoot;

    private FileSystemWatcher watcher;
    private readonly IJobQueue jobQueue;
    private readonly SystemProvider systemProvider;
    private FileStream fileStream;

    public EditorGame(IGameCreationContext context, IJobQueue jobQueue, SystemProvider systemProvider) : base(context)
    {
        IsMouseVisible = true;
        this.Window.AllowUserResizing = true;
        this.Window.IsBorderless = false;
        this.Window.Title = "DualBlade Editor";
        Context.GameEngine.GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
        Context.GameEngine.GraphicsDeviceManager.PreferredBackBufferHeight = 720;
        this.jobQueue = jobQueue;
        this.systemProvider = systemProvider;
    }

    protected override void InitializeGlobalSystems()
    {
        World.AddSystem<RenderSystem>();
        World.AddSystem<CameraSystem>();
        World.AddSystem<InputSystem>();
        World.AddSystem<RenderSelectSystem>();
        World.AddSystem<SelectableSystem>();
        World.AddSystem<MyraDesktopSystem>();
        World.AddSystem(new EditorUiSystem(Context, systemProvider, Context.ServiceProvider.GetRequiredService<IJobQueue>()));
    }

    protected override void Initialize()
    {
        base.Initialize();

        sceneGenerator = new SceneGenerator(Context);
        var path = @"E:\\GameDev\DualBladeEngine\Source\Examples\ExampleGame\Scenes\TestScene.scene.yaml";
        sceneRoot = new SceneParser().ParseScene(System.IO.File.ReadAllText(path));
        var scene = sceneGenerator.Create(sceneRoot);

        this.SceneManager.AddSceneExclusively(scene);

        this.World.AddEntity(new EditorUiEntity(sceneRoot, OnChange));

        string directoryPath = Path.GetDirectoryName(path);
        var fileName = Path.GetFileName(path);
        watcher = new FileSystemWatcher(directoryPath, fileName)
        {
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };
        watcher.Changed += Filewatcher_Changed;

        fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    private void Filewatcher_Changed(object sender, FileSystemEventArgs e)
    {
        try
        {
            lock (fileStream)
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(fileStream);
                var text = reader.ReadToEnd();

                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                sceneRoot = new SceneParser().ParseScene(text);
                var scene = sceneGenerator.Create(sceneRoot);

                jobQueue.Enqueue(() => this.SceneManager.AddSceneExclusively(scene));
            }
        }
        catch (Exception)
        {

        }
    }

    private void OnChange()
    {
        var scene = sceneGenerator.Create(sceneRoot);
        this.SceneManager.AddSceneExclusively(scene);
    }

    private void AddEditorScene(IGameScene gameScene)
    {
        var root = gameScene.Root;
        World.AddEntity(root);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        base.Draw(gameTime);
    }
}
