using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopup : Popup
{
    private readonly float NewScale = 1f;
    private readonly float Duration = 1f;

    [SerializeField] private UpperPart _upperPart;
    [SerializeField] private Button _button;

    private SaveLoader _saveLoader;

    private void OnEnable()
    {
        if (_upperPart == null)
        {
            throw new ArgumentNullException(nameof(_upperPart));
        }

        if (_button == null)
        {
            throw new ArgumentNullException(nameof(_button));
        }

        _upperPart.transform.localScale = Vector3.zero;

        _upperPart.transform.DOScale(NewScale, Duration)
            .SetEase(Ease.OutBounce);

        _button.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Restart);
    }

    public void Init(SaveLoader saveLoader)
    {
        if (saveLoader == null)
        {
            throw new ArgumentNullException(nameof(saveLoader));
        }

        _saveLoader = saveLoader;
        enabled = true;
    }

    private void Restart()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        _saveLoader.Save();
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}