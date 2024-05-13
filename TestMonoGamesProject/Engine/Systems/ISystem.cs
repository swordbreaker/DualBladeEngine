﻿using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Systems
{
    public interface ISystem
    {
        void Update(GameTime gameTime, IGameEngine gameEngine);

        void Draw(GameTime gameTime, IGameEngine gameEngine);
        void Initialize(IGameEngine gameEngine);
    }
}
