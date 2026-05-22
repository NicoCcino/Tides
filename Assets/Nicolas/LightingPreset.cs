using UnityEngine;

[CreateAssetMenu(fileName = "LightingPreset", menuName = "Scriptable Objects/LightingPreset")]
public class LightingPreset : ScriptableObject
{
    public Gradient ambientColor;
    public Gradient directionalColor;
    public Gradient fogColor;
}
