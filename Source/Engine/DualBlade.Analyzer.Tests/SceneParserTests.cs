using FluentAssertions;

namespace DualBlade.Analyzer.Tests;

public class SceneParserTests
{
    private const string TestYaml =
"""
Entities:
    - Type: TransformEntity
      Name: Ball
      Components:
      - Type: TransformComponent
        Properties:
            Position: [0, 0]
            Rotation: 0
            Scale: [1, 1]
      - Type: RenderComponent
        Properties:
            Texture: ball
      Children:
      - Type: TransformEntity
        Name: Ball
        Components:
        - Type: TransformComponent
          Properties:
            Position: [0, 0]
            Rotation: 0
            Scale: [1, 1]
        - Type: RenderComponent
          Properties:
            Texture: ball
            Color: White
            Origin: [0, 0]
""";

    [Fact]
    public void ParseSceneTest()
    {
        var parser = new SceneParser();
        var result = parser.ParseScene(TestYaml);

        result.Entities.Should().NotBeEmpty();
        result.Entities.Should().HaveCount(1);
    }
}