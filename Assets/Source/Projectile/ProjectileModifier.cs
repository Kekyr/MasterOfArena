using System;
using DG.Tweening;
using UnityEngine;

public class ProjectileModifier : MonoBehaviour
{
    private readonly float ScaleModifier = 1.3f;
    private readonly float ScaleDuration = 0.05f;

    private Character _character;
    private Vector3 _originalScale;

    private void OnEnable()
    {
        _character.Throwed += OnThrow;
    }

    private void OnDisable()
    {
        _character.Throwed -= OnThrow;
    }

    public void Init(Character character)
    {
        if (character == null)
        {
            throw new ArgumentNullException(nameof(character));
        }

        _character = character;
        enabled = true;
    }

    public void ChangeScale()
    {
        Vector3 newScale = _originalScale * ScaleModifier;

        transform.DOScale(newScale, ScaleDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DOScale(_originalScale, ScaleDuration)
                    .SetEase(Ease.InOutSine);
            });
    }



    private void OnThrow(Transform newParent)
    {
        _originalScale = transform.localScale;
    }
}