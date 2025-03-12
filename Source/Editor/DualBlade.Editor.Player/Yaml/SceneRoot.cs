using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace DualBlade.Analyzer;
public class SceneRoot
{
    [YamlMember(Alias = "$schema")]
    public string Schema { get; set; }
    public List<string> AdditionalUsings { get; set; }
    public List<EntityDto> Entities { get; set; }
}
