﻿// <auto-generated>
// Do not edit this file yourself!
//
// This code was generated by Stride Shader Mixin Code Generator.
// To generate it yourself, please install Stride.VisualStudio.Package .vsix
// and re-save the associated .sdfx.
// </auto-generated>

using System;
using Stride.Core;
using Stride.Rendering;
using Stride.Graphics;
using Stride.Shaders;
using Stride.Core.Mathematics;
using Buffer = Stride.Graphics.Buffer;

namespace Stride.Rendering
{
    public static partial class DropShadow_TextureFXKeys
    {
        public static readonly ValueParameterKey<float> Blur = ParameterKeys.NewValue<float>(0.5f);
        public static readonly ValueParameterKey<float> Offset = ParameterKeys.NewValue<float>(0.05f);
        public static readonly ValueParameterKey<float> Angle = ParameterKeys.NewValue<float>(0.9f);
        public static readonly ValueParameterKey<Color4> Color = ParameterKeys.NewValue<Color4>(new Color4(0.0f,0.0f,0.0f,1.0f));
        public static readonly ValueParameterKey<float> Alpha = ParameterKeys.NewValue<float>(1.0f);
        public static readonly ValueParameterKey<float> Extension = ParameterKeys.NewValue<float>(0.0f);
    }
}
