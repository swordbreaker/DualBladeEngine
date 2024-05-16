//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Myra;
//using Myra.Graphics2D.UI;
//using System.Drawing;
//using System.IO;
//using TestMonoGamesProject.Engine.Systems;
//using TestMonoGamesProject.Engine.World;
//using TestMonoGamesProject.Engine.Worlds;
//using TestMonoGamesProject.Entities;
//using TestMonoGamesProject.Systems;
//using Color = Microsoft.Xna.Framework.Color;

//namespace TestMonoGamesProject
//{
//    public class MainGame : Game
//    {
//        private IGameEngine _gameEngine;
//        private IWorld _gameWorld;
//        private Desktop _desktop;
//        private RectangleF _ground;

//        public MainGame(WorldFactory worldFactory, IGameEngineFactory gameEngineFactory)
//        {
//            _gameEngine = gameEngineFactory.Create(this);
//            _gameWorld = worldFactory.Create(_gameEngine);
//            Content.RootDirectory = "Content";
//            IsMouseVisible = true;
//        }

//        protected override void Initialize()
//        {
//            _gameEngine.Initialize();

//            //var (w, h) = _gameWorld.Size;

//            //_ground = new RectangleF(
//            //        0,
//            //        h - 1,
//            //        w,
//            //        1);

//            //_gameWorld.PhysicsManager.AddColliders(new RectCollider(_ground));
//            base.Initialize();
//        }

//        protected override void LoadContent()
//        {
//            var (w, h) = _gameEngine.GameSize;
//            var ballPos = new Vector2(
//                w / 2,
//                h / 2);

//            _gameWorld.AddEntity(new BallEntity(_gameEngine, ballPos) { World = _gameWorld });
//            _gameWorld.AddEntity(new GroundEntity(_gameEngine) { World = _gameWorld});
//            _gameWorld.AddSystem<RenderSystem>();
//            _gameWorld.AddSystem<KinematicSystem>();
//            _gameWorld.AddSystem<ColliderSystem>();
//            _gameWorld.AddSystem<BallSystem>();
//            _gameWorld.AddSystems(new InputSystem());

//            MyraEnvironment.Game = this;
//            string data = File.ReadAllText("Content/Menu.xmmp");
//            var project = Project.LoadFromXml(data);
//            _desktop = new Desktop
//            {
//                Root = project.Root
//            };
//        }

//        protected override void Update(GameTime gameTime)
//        {
//            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
//                Exit();

//            _gameWorld.Update(gameTime);
//            base.Update(gameTime);
//        }

//        protected override void Draw(GameTime gameTime)
//        {
//            GraphicsDevice.Clear(Color.CornflowerBlue);
//            _gameWorld.Draw(gameTime);

//            _desktop.Render();
//            base.Draw(gameTime);
//        }
//    }
//}
