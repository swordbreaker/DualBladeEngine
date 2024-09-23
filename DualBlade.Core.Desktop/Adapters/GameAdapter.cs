using DualBlade.Core.Shared.MonoGameInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualBlade.Core.Desktop.Adapters;

public class GameAdapter : Game, IMonoGame
{
    public void Draw(IGameTime gameTime) =>
    public void InitializeGlobalSystems() => throw new NotImplementedException();
    public void Update(IGameTime gameTime) => throw new NotImplementedException();
    void IMonoGame.Initialize() => throw new NotImplementedException();
}
