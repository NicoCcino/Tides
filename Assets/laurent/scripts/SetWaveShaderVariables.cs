using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetWaveShaderVariables : MonoBehaviour
{
    public string wavePositionName = "_WavePos";
    public string waveDirectionName = "_WaveDir";
    private MaterialPropertyBlock propertyBlock;
    private int wavePosInt;
    private int waveDirInt;
    public MeshRenderer waveRenderer;

    private void OnEnable()
    {
        propertyBlock = new MaterialPropertyBlock();
        //InvokeRepeating("SetVariables", 0.0f, 0.05f);
        wavePosInt = propertyBlock.GetInteger(wavePositionName);
        waveDirInt = propertyBlock.GetInteger(waveDirectionName);
    }

    private void Update()
    {
        SetVariables();
    }

    public void SetVariables()
    {
        propertyBlock.SetVector(wavePositionName, gameObject.transform.position + new Vector3(0, 0.7f, 0));
        propertyBlock.SetVector(waveDirectionName, gameObject.transform.TransformDirection(0,0,-1));
        if(waveRenderer != null)
            waveRenderer.SetPropertyBlock(propertyBlock);
    }
}
