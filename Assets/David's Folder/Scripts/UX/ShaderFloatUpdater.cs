using UnityEngine;


public class ShaderFloatUpdater : BaseShaderPropertyUpdater
{
    [Tooltip("Maps the 0-1 tracked value to a custom float output (e.g., a fill value)")]
    [SerializeField] private AnimationCurve valueCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private float evaluatedFloat;

    protected override void EvaluateTrackedValue(float currentValue)
    {
        // The curve allows you to remap the 0-1 input to any desired float output range
        evaluatedFloat = valueCurve.Evaluate(currentValue);
    }

    protected override void ApplyToPropertyBlock(MaterialPropertyBlock block, int propertyId)
    {
        block.SetFloat(propertyId, evaluatedFloat);
    }
}
