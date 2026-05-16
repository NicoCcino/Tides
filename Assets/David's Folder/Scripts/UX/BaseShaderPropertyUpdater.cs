using UnityEngine;



public abstract class BaseShaderPropertyUpdater : MonoBehaviour
{
    [SerializeField] protected float trackedFloatValue;
    [SerializeField] protected RendererShaderMapping[] affectedRenderers;

    private MaterialPropertyBlock materialPropertyBlock;

    protected virtual void Awake()
    {
        // Initialize the block once to prevent garbage collection spikes
        materialPropertyBlock = new MaterialPropertyBlock();

        // Pre-calculate the integer IDs for performance
        for (int i = 0; i < affectedRenderers.Length; i++)
        {
            if (!string.IsNullOrEmpty(affectedRenderers[i].shaderPropertyId))
            {
                affectedRenderers[i].propertyId = Shader.PropertyToID(affectedRenderers[i].shaderPropertyId);
            }
        }
    }

    protected virtual void Update()
    {
        if (trackedFloatValue == null) return;

        // 1. Evaluate the tracked value once per frame
        EvaluateTrackedValue(trackedFloatValue);

        // 2. Apply to all renderers
        for (int i = 0; i < affectedRenderers.Length; i++)
        {
            var mapping = affectedRenderers[i];
            if (mapping.renderer == null) continue;

            // Get existing properties to avoid overwriting other MaterialPropertyBlock changes
            mapping.renderer.GetPropertyBlock(materialPropertyBlock);

            // Let the child class set the specific property type (Float, Color, etc.)
            ApplyToPropertyBlock(materialPropertyBlock, mapping.propertyId);

            // Apply the changes back to the renderer
            mapping.renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }

    /// <summary>
    /// Called once per frame before iterating through renderers.
    /// Calculate your Gradient/Curve evaluation here.
    /// </summary>
    protected abstract void EvaluateTrackedValue(float currentValue);

    /// <summary>
    /// Called for each renderer. Apply the evaluated value to the property block here.
    /// </summary>
    protected abstract void ApplyToPropertyBlock(MaterialPropertyBlock block, int propertyId);
}
