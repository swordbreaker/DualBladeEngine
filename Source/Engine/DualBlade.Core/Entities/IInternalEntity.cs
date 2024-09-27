using DualBlade.Core.Components;
using DualBlade.Core.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualBlade.Core.Entities;

public interface IInternalEntity
{
    public int Id { get; }
    void Init(TestWorld world, int id);
    void AddComponent<TComponent>(int id);
    public Span<IComponent> Components { get; }
}
