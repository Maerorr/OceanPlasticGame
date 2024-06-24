using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShadersController : MonoBehaviour
{
    public List<Material> outlineMaterials;
    private int enableID = Shader.PropertyToID("_UseOutline");

    private void Start()
    {
        if (StaticGameData.instance.showOutline)
        {
            EnableOutline();
        }
        else
        {
            DisableOutline();
        }
    }

    public void EnableOutline()
    {
        foreach (var mat in outlineMaterials)
        {
            mat.SetInt(enableID, 1);
        }
    }
    
    public void DisableOutline()
    {
        foreach (var mat in outlineMaterials)
        {
            mat.SetInt(enableID, 0);
        }
    }
}
