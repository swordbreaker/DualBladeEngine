using System.Collections.Generic;
using System.ComponentModel;

namespace DualBlade.Analyzer;

public class EntityDto
{
    public string Name { get; set; }
    public string Type { get; set; }

    public List<object> Ctor { get; set; } = [];
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
