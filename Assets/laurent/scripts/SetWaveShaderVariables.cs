using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetWaveShaderVariables : MonoBehaviour
{
    public string wavePositionName = "_WavePos";
    public string waveDirectionName = "_WaveDir";
    public string restHeightName = "_RestHeight";
    public string waveHeightName = "_WaveHeight";
    public string waveGradientWidthName = "_WaveHeight";
    private MaterialPropertyBlock propertyBlock;
    private int wavePosInt;
    private int waveDirInt;
    public MeshRenderer waveRenderer;
    public int restHeight = -3;
    public int waveHeight = 0;
    public float waveGradientWidth = 5;

    private void OnEnable()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        SetVariables();
    }

    public void SetVariables()
    {
        propertyBlock.SetVector(wavePositionName, gameObject.transform.position + new Vector3(0, 0.7f, 0));
        propertyBlock.SetVector(waveDirectionName, gameObject.transform.TransformDirection(0,0,-1));
        propertyBlock.SetFloat(restHeightName, restHeight);
        propertyBlock.SetFloat(waveHeightName, waveHeight);
        propertyBlock.SetFloat(waveGradientWidthName, waveGradientWidth);
        if (waveRenderer != null)
            waveRenderer.SetPropertyBlock(propertyBlock);
    }
}


