using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class MainButton : MonoBehaviour
    {
        private readonly float _duration = 1f;

        [SerializeField] private MainPopup _popup;
        [SerializeField] private float _newScale;

        private Button _button;

        protected virtual void OnEnable()
        {
            if (_popup == null)
            {
                throw new ArgumentNullException(nameof(_popup));
            }

            _button = GetComponent<Button>();

            transform.localScale = Vector3.zero;
            transform.DOScale(_newScale, _duration)
                .SetEase(Ease.InOutSine);

            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        protected virtual void OnClick()
        {
            _popup.gameObject.SetActive(true);
        }
    }
}