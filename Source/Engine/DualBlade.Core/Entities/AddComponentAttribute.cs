﻿namespace DualBlade.Core.Entities;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class AddComponentAttribute<TComponent> : Attribute
{
}