using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class ProjectileHelper : MonoBehaviour
{
    private readonly float ScaleModifier = 1.3f;
    private readonly float ScaleDuration = 0.05f;
    private readonly float ColorDuration = 0.02f;

    [SerializeField] private MeshRenderer _meshRenderer;

    private Catcher _catcher;
    private Vector3 _originalScale;

    private void OnEnable()
    {
        if (_meshRenderer == null)
            throw new ArgumentNullException(nameof(_meshRenderer));

        _catcher.Throwed += OnThrow;
    }

    private void OnDisable()
    {
        _catcher.Throwed -= OnThrow;
    }

    public void Init(Catcher catcher)
    {
        if (catcher == null)
            throw new ArgumentNullException(nameof(catcher));

        _catcher = catcher;
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

    public void ChangeColor()
    {
        Sequence sequence = DOTween.Sequence();
        List<Color> startColors = new List<Color>();

        for (int i = 0; i < _meshRenderer.materials.Length; i++)
        {
            Material material = _meshRenderer.materials[i];
            startColors.Add(material.color);
            sequence.Append(material.DOColor(Color.white, ColorDuration)
                .SetEase(Ease.InOutSine));
        }

        sequence.OnComplete(() =>
        {
            for (int i = 0; i < _meshRenderer.materials.Length; i++)
            {
                Material material = _meshRenderer.materials[i];
                material.DOColor(startColors[i], ColorDuration)
                    .SetEase(Ease.InOutSine);
            }
        });
    }

    private void OnThrow(Transform newParent)
    {
        _originalScale = transform.localScale;
    }
}