using System.Collections.Generic;
using System.ComponentModel;

namespace DualBlade.Analyzer;

public class EntityDto
{
    public string Name { get; set; }
    public string Type { get; set; }
    public float[] Position { get; set; } = [0, 0];
    public float Rotation { get; set; } = 0;
    public float[] Scale { get; set; } = [0, 0];
    public List<ComponentDto> Components { get; set; } = [];
    public List<EntityDto> Children { get; set; } = [];

    [Browsable(false)]
    public Dictionary<string, object> Properties { get; set; } = [];
}

public class ComponentDto
{
    public string Type { get; set; }
    public Dictionary<string, object> Properties { get; set; } = [];
}
