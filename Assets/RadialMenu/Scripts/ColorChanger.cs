using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Material pinkMaterial;
    public Material greyMaterial;

    public void SetPink()
    {
        SetMaterial(pinkMaterial);
    }

    public void SetGrey()
    {
        SetMaterial(greyMaterial);
    }

    private void SetMaterial(Material newMaterial)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = newMaterial;
    }
}
