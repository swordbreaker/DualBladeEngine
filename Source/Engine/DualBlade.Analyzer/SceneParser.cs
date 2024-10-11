using System;
namespace DualBlade.Analyzer;
public class SceneParser
{
    public SceneRoot ParseScene(string text)
    {
        try
        {
            var data = new YamlDotNet.Serialization.Deserializer().Deserialize<SceneRoot>(text);
            return data;
        }
        catch (Exception)
        {
            throw;
            return null;
        }
    }
}
