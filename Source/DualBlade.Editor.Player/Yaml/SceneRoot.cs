using System.Collections.Generic;

namespace DualBlade.Analyzer;
public class SceneRoot
{
    public List<string> AdditionalUsings { get; set; }
    public List<EntityDto> Entities { get; set; }
}
