using System;
using System.Collections.Generic;

namespace MonoGamesEngine.Analyzer;
public class SceneParser
{
    public Entity? ParseScene(string text)
    {
        try
        {
            var root = new Entity() { Type = "RootEntity" };

            var data = new YamlDotNet.Serialization.Deserializer().Deserialize<List<Entity>>(text);

            root.Children.AddRange(data);
            return root;
        }
        catch(Exception e)
        {
            return null;
        }
    }
}
