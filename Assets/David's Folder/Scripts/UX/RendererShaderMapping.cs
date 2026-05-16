using System;
using UnityEngine;


[Serializable]
public struct RendererShaderMapping
{
    public Renderer renderer;

    [Tooltip("The string name of the shader property (e.g., _BaseColor, _FillAmount)")]
    public string shaderPropertyId;

    // Cached integer ID for better performance
    [HideInInspector] public int propertyId;
}
