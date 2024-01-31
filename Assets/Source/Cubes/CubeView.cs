using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CubeView : MonoBehaviour
{
    private readonly float NewScale = 1f;
    private readonly float YModifier = 0.5f;
    private readonly float Delay = 0.05f;
    private readonly float Duration = 0.05f;
    private readonly float MoveYDuration = 3f;

    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private DamageMark _mark;

    private TextMeshProUGUI _markTextMesh;
    private Image _markImage;

    private string _text;

    private void OnEnable()
    {
        if (_textMesh == null)
            throw new ArgumentNullException(nameof(_textMesh));

        if (_mark == null)
            throw new ArgumentNullException(nameof(_mark));

        _markTextMesh = _mark.GetComponentInChildren<TextMeshProUGUI>();
        _markImage = _mark.GetComponentInChildren<Image>();

        _textMesh.text = _text;
        _markTextMesh.text = _text;

        transform.localScale = Vector3.zero;
        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(Delay);
    }

    public void Init(string text)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        _text = text;
        enabled = true;
    }

    public void OnCollision(Color newColor)
    {
        _textMesh.gameObject.SetActive(false);
        _markTextMesh.color = newColor;
        _markImage.color = newColor;
        _mark.gameObject.SetActive(true);
        _mark.gameObject.transform.DOMoveY(_mark.gameObject.transform.position.y + YModifier, MoveYDuration)
            .SetEase(Ease.Linear);
    }
}