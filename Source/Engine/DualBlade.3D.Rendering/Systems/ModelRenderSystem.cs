using DualBlade._3D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade._3D.Rendering.Systems;

public class ModelRenderSystem(IGameContext context)
    : ComponentSystem<TransformComponent3D, ModelComponent>(context)
{
    private readonly ICameraService cameraService = context.GameEngine.CameraService;

    protected override void Draw(
        TransformComponent3D transform,
        ModelComponent modelCoponent,
        IEntity entity,
        GameTime gameTime)
    {
        // The world matrix, also known as the model matrix, is responsible for transforming objects
        // from their local coordinate space (model space) to world space.
        // This matrix encodes the object's position, rotation, and scale in the 3D world.
        var world = Matrix.CreateScale(transform.Scale) *
                    Matrix.CreateFromQuaternion(transform.Rotation) *
                    Matrix.CreateTranslation(transform.Position);

        modelCoponent.Model.Draw(world, cameraService.ViewMatrix, cameraService.ProjectionMatrix);
    }
}