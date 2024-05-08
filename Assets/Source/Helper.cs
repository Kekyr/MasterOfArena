using DG.Tweening;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public void ChangeMeshColor(MeshRenderer meshRenderer, Color tempColor, float duration)
    {
        foreach (Material material in meshRenderer.materials)
        {
            ChangeMaterialColor(material, tempColor, duration);
        }
        
        
    }

    private void ChangeMaterialColor(Material material, Color tempcolor, float duration)
    {
        Color startColor = material.color;

        material.DOColor(tempcolor, duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                material.DOColor(startColor, duration)
                    .SetEase(Ease.InOutSine);
            });
    }
}