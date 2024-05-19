using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameEngine.Engine.Extensions;
public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector2 vector) =>
        new Vector3(vector.X, vector.Y, 0);
}
