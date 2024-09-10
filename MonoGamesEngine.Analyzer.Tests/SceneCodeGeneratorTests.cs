using FluentAssertions;
using Xunit.Abstractions;

namespace MonoGamesEngine.Analyzer.Tests;

public class SceneCodeGeneratorTests(ITestOutputHelper Output)
{
    [Fact]
    public void GenerateCodeTest()
    {
        // arrange
        var generator = new SceneCodeGenerator();
        var entities = new List<IEntity>
        {
            new Entity
            {
                Name = "Ball",
                Type = "TransformEntity",
                Position = new float[] { 0, 0 },
                Rotation = 0,
                Scale = new float[] { 1, 1 },
                Components = new List<Component>
                {
                    new Component
                    {
                        Type = "RenderComponent",
                        Properties = new Dictionary<string, object>
                        {
                            { "Texture", "ball" }
                        }
                    }
                },
                Children = new List<Entity>
                {
                    new Entity
                    {
                        Name = "Ball",
                        Type = "TransformEntity",
                        Position = new float[] { 0, 0 },
                        Rotation = 0,
                        Scale = new float[] { 1, 1 },
                        Components = new List<Component>
                        {
                            new Component
                            {
                                Type = "RenderComponent",
                                Properties = new Dictionary<string, object>
                                {
                                    { "Texture", "ball" },
                                    { "Color", "White" },
                                    { "Origin", new float[] { 0, 0 } }
                                }
                            }
                        }
                    }
                }
            }
        };

        // act
        var result = generator.GenerateCode(entities, "TestScene");

        // assert
        result.Should().NotBeNullOrEmpty();

        result.Should().Contain("internal class TestScene : YamlGameScene");

        Output.WriteLine(result);
    }
}