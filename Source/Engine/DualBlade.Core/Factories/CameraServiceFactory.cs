﻿using DualBlade.Core.Services;

namespace DualBlade.Core.Factories;
public sealed class CameraServiceFactory : ICameraServiceFactory
{
    public ICameraService Create(GraphicsDeviceManager graphicsDeviceManager, IWorldToPixelConverter worldToPixelConverter) =>
        new CameraService(graphicsDeviceManager, worldToPixelConverter);
}