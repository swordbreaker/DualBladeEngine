using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGamesEngine.Analyzer;
public class SceneParser
{
    public IEnumerable<IEntity> ParseScene(string text)
    {
        try
        {
            var data = new YamlDotNet.Serialization.Deserializer().Deserialize<List<Entity>>(text);
            return data.Cast<IEntity>();
        }
        catch (Exception)
        {
            return null;
        }
    }
}
