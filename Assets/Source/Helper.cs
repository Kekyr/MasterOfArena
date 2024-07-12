using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public void ChangeMeshColor(MeshRenderer meshRenderer, Color tempColor, float duration)
    {
        foreach (Material material in meshRenderer.materials)
        {
            if (material.mainTexture == null)
            {
                ChangeMaterialColor(material, tempColor, duration);
            }
            else
            {
                StartCoroutine(ChangeMaterialColor(material, duration));
            }
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

    private IEnumerator ChangeMaterialColor(Material material, float duration)
    {
        Texture texture = material.mainTexture;
        material.mainTexture = null;

        yield return new WaitForSeconds(duration);

        material.mainTexture = texture;
    }
}