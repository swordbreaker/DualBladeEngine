using System;
using System.Collections.Generic;

namespace MonoGamesEngine.Analyzer;
public class SceneParser
{
    public IList<Entity> ParseScene(string text)
    {
        try
        {
            var data = new YamlDotNet.Serialization.Deserializer().Deserialize<List<Entity>>(text);
            return data;
        }
        catch(Exception e)
        {
            return null;
        }
    }
}
