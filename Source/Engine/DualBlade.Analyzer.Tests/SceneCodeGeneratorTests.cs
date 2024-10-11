//using FluentAssertions;
//using Xunit.Abstractions;

//namespace DualBlade.Analyzer.Tests;

//public class SceneCodeGeneratorTests(ITestOutputHelper Output)
//{
//    [Fact]
//    public void GenerateCodeTest()
//    {
//        // arrange
//        var generator = new SceneCodeGenerator();
//        var entities = new List<Entity>
//        {
//            new() {
//                Name = "Ball",
//                Type = "TransformEntity",
//                Position = [0, 0],
//                Rotation = 0,
//                Scale = [1, 1],
//                Components =
//                [
//                    new Component
//                    {
//                        Type = "RenderComponent",
//                        Properties = new Dictionary<string, object>
//                        {
//                            { "Texture", "ball" }
//                        }
//                    }
//                ],
//                Children =
//                [
//                    new Entity
//                    {
//                        Name = "Ball",
//                        Type = "TransformEntity",
//                        Position = [0, 0],
//                        Rotation = 0,
//                        Scale = [1, 1],
//                        Components =
//                        [
//                            new Component
//                            {
//                                Type = "RenderComponent",
//                                Properties = new Dictionary<string, object>
//                                {
//                                    { "Texture", "ball" },
//                                    { "Color", "White" },
//                                    { "Origin", new float[] { 0, 0 } }
//                                }
//                            }
//                        ]
//                    }
//                ]
//            }
//        };

//        var root = new SceneRoot
//        {
//            Entities = [.. entities],
//            AdditionalUsings = [],
//        };

//        // act
//        var result = generator.GenerateCode(root, "TestScene");

//        // assert
//        result.Should().NotBeNullOrEmpty();

//        result.Should().Contain("internal class TestScene : YamlGameScene");

//        Output.WriteLine(result);
//    }
//}