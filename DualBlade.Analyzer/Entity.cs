using System.Collections.Generic;

namespace DualBlade.Analyzer;

[GenerateAutomaticInterface]
public class Entity : IEntity
{
    public string Name { get; set; }
    public string Type { get; set; }
    public float[] Position { get; set; } = [0, 0];
    public float Rotation { get; set; } = 0;
    public float[] Scale { get; set; } = [0, 0];
    public List<Component> Components { get; set; } = [];
    public List<Entity> Children { get; set; } = [];
    public Dictionary<string, object> Properties { get; set; } = [];
}

[GenerateAutomaticInterface]
public class Component : IComponent
{
    public string Type { get; set; }
    public Dictionary<string, object> Properties { get; set; } = [];
}
