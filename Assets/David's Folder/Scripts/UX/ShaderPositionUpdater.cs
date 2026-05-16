using UnityEngine;

namespace IFCE.ValueTracking.UX
{
    public class ShaderPositionUpdater : MonoBehaviour
    {
        [Tooltip("The transform whose position will be sent to the shader.")]
        [SerializeField] private Transform targetTransform;

        [Tooltip("Renderers and their corresponding shader property names (e.g., _TargetPosition).")]
        [SerializeField] private RendererShaderMapping[] affectedRenderers;

        private MaterialPropertyBlock materialPropertyBlock;

        private void Awake()
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

        private void Start()
        {
            if (targetTransform == null) return;

            // Cache the position once per frame
            Vector3 currentPosition = targetTransform.position;

            for (int i = 0; i < affectedRenderers.Length; i++)
            {
                var mapping = affectedRenderers[i];
                if (mapping.renderer == null) continue;

                // Get existing properties to avoid overwriting other MaterialPropertyBlock changes
                mapping.renderer.GetPropertyBlock(materialPropertyBlock);

                // Apply the position. Note: Shaders process vectors as Vector4. 
                // Unity automatically handles the Vector3 to Vector4 conversion here.
                materialPropertyBlock.SetVector(mapping.propertyId, currentPosition);

                // Apply the changes back to the renderer
                mapping.renderer.SetPropertyBlock(materialPropertyBlock);
            }
        }
    }
}