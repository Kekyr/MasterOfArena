using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinView : MonoBehaviour
{
    private readonly float NewPositionY = -50f;
    private readonly float Duration = 1f;
    private readonly float NewScale = 1.3f;

    [SerializeField] private GameObject _focus;
    [SerializeField] private RectTransform _visual;
    [SerializeField] private GameObject _cost;
    [SerializeField] private Image _image;
    [SerializeField] private SkinSO _data;

    private Image _background;

    private TextMeshProUGUI _costTextMesh;
    private Button _button;

    public event Action<SkinView> Selected;

    private void OnEnable()
    {
        if (_visual == null)
        {
            throw new ArgumentNullException(nameof(_visual));
        }

        if (_image == null)
        {
            throw new ArgumentNullException(nameof(_image));
        }

        if (_cost == null)
        {
            throw new ArgumentNullException(nameof(_cost));
        }

        if (_focus == null)
        {
            throw new ArgumentNullException(nameof(_focus));
        }

        _background = GetComponent<Image>();
        _button = GetComponent<Button>();
        _costTextMesh = _cost.GetComponentInChildren<TextMeshProUGUI>();

        _image.sprite = _data.Image;
        _costTextMesh.text = _data.Cost.ToString();

        InitStatus();

        _button.onClick.AddListener(OnStatusChanged);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnStatusChanged);
    }

    public void Init(SkinSO data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        _data = data;
        enabled = true;
    }

    public void Select()
    {
        _focus.SetActive(true);
        _data.UpgradeStatus();
    }

    public void Deselect()
    {
        _focus.SetActive(false);
        _data.DowngradeStatus();
    }

    private void OnStatusChanged()
    {
        switch (_data.Status)
        {
            case State.NotBought:
                OnBought();
                _data.UpgradeStatus();
                break;

            case State.Bought:
                Selected?.Invoke(this);
                break;
        }
    }

    private void InitStatus()
    {
        switch (_data.Status)
        {
            case State.Bought:
                OnBought();
                break;

            case State.Selected:
                OnBought();
                Selected?.Invoke(this);
                break;
        }
    }

    private void OnBought()
    {
        _cost.SetActive(false);
        _background.color = Color.white;
        _visual.localPosition = new Vector3(_visual.localPosition.x, _visual.localPosition.y + NewPositionY,
            _visual.localPosition.z);
        _visual.localScale = new Vector3(NewScale, NewScale, NewScale);
    }
}