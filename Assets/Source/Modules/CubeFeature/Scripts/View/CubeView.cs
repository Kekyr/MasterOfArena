using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CubeFeature
{
    public class CubeView : MonoBehaviour
    {
        private readonly float _yModifier = 0.5f;
        private readonly float _delay = 0.05f;
        private readonly float _duration = 0.05f;
        private readonly float _moveYDuration = 3f;

        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private GameObject _mark;

        private TextMeshProUGUI _markTextMesh;
        private Image _markImage;

        private string _text;
        private Vector3 _startScale;

        private void OnEnable()
        {
            if (_textMesh == null)
            {
                throw new ArgumentNullException(nameof(_textMesh));
            }

            if (_mark == null)
            {
                throw new ArgumentNullException(nameof(_mark));
            }

            _markTextMesh = _mark.GetComponentInChildren<TextMeshProUGUI>();
            _markImage = _mark.GetComponentInChildren<Image>();

            _textMesh.text = _text;
            _markTextMesh.text = _text;

            _startScale = transform.localScale;
            transform.localScale = Vector3.zero;

            transform.DOScale(_startScale, _duration)
                .SetEase(Ease.InOutSine)
                .SetDelay(_delay);
        }

        public void Init(string text)
        {
            _text = text;
            enabled = true;
        }

        public void OnCollision(Color newColor)
        {
            _textMesh.gameObject.SetActive(false);
            _markTextMesh.color = newColor;
            _markImage.color = newColor;

            _mark.SetActive(true);
            _mark.transform.DOMoveY(_mark.gameObject.transform.position.y + _yModifier, _moveYDuration)
                .SetEase(Ease.Linear);
        }
    }
}