using FluentAssertions;

namespace MonoGamesEngine.Analyzer.Tests;

public class SceneParserTests
{
    private const string TestYaml =
"""
- Type: TransformEntity
  Name: Ball
  Position: [0, 0]
  Rotation: 0
  Scale: [1, 1]
  Components:
  - Type: RenderComponent
    Properties:
        Texture: ball
  Children:
  - Type: TransformEntity
    Name: Ball
    Position: [0, 0]
    Rotation: 0
    Scale: [1, 1]
    Components:
    - Type: RenderComponent
      Properties:
        Texture: ball
        Color: White
        Origin: [0, 0]
""";

    [Fact()]
    public void ParseSceneTest()
    {
        var parser = new SceneParser();
        var result = parser.ParseScene(TestYaml);

        result.Should().NotBeNull();
        result.Children.Should().HaveCount(1);
    }
}