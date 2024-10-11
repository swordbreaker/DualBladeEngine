using System.Collections.Generic;

namespace DualBlade.Analyzer;

public class YamlEntity
{
    public string Name { get; set; }
    public string Type { get; set; }
    public List<YamlComponent> Components { get; set; } = [];
    public List<YamlEntity> Children { get; set; } = [];
    public List<object> Ctor { get; set; } = [];
    public Dictionary<string, object> Properties { get; set; } = [];
}

public class YamlComponent
{
    public string Type { get; set; }
    public Dictionary<string, object> Properties { get; set; } = [];
}
