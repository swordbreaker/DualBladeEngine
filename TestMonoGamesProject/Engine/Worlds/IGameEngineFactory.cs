﻿using Microsoft.Xna.Framework;

namespace TestMonoGamesProject.Engine.Worlds
{
    public interface IGameEngineFactory
    {
        IGameEngine Create(Game game);
    }
}