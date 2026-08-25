using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ShopObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private Material litMaterial;

    public void DisplayTransparent()
    {
        meshRenderer.material = transparentMaterial;
    }

    public void DisplayLit()
    {
        meshRenderer.material = litMaterial;
    }
}
