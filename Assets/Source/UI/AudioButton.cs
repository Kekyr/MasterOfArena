using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private ImageSO _imageSO;

    public Action Switched;

    private void OnEnable()
    {
        if (_button == null)
            throw new ArgumentNullException(nameof(_button));

        if (_image == null)
            throw new ArgumentNullException(nameof(_image));

        if (_imageSO == null)
            throw new ArgumentNullException(nameof(_imageSO));

        _image.sprite = _imageSO.CurrentSprite;
        _button.onClick.AddListener(Switch);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Switch);
    }

    protected virtual void Switch()
    {
        _image.sprite = _imageSO.Switch();
        Switched?.Invoke();
    }
}