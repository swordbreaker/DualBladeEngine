using DualBlade._3D.Rendering.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade._3D.Rendering.Systems;

public class ModelRenderSystem(IGameContext context) : ComponentSystem<TransformComponent3D, ModelComponent>(context)
{
    
}