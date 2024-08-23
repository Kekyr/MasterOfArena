using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : MainPopup
{
    [SerializeField] private SkinView _prefab;

    private GridLayoutGroup _content;
    private List<SkinView> _skinViewes;
    private SkinSO[] _skins;
    private SkinView _selectedSkinView;

    private void OnEnable()
    {
        if (_prefab == null)
        {
            throw new ArgumentNullException(nameof(_prefab));
        }

        _content = GetComponentInChildren<GridLayoutGroup>();
        _skinViewes = new List<SkinView>();

        foreach (SkinSO skin in _skins)
        {
            SkinView view = Instantiate(_prefab, _content.transform);
            view.Selected += OnSelected;
            view.Init(skin);
            _skinViewes.Add(view);
        }
    }

    private void OnDisable()
    {
        foreach (SkinView skinView in _skinViewes)
        {
            skinView.Selected -= OnSelected;
        }
    }

    public void Init(SkinSO[] skins)
    {
        if (skins.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skins));
        }

        _skins = skins;
        enabled = true;
    }

    private void OnSelected(SkinView skinView)
    {
        if (_selectedSkinView == null)
        {
            skinView.Select();
            _selectedSkinView = skinView;
            return;
        }

        _selectedSkinView.Deselect();
        skinView.Select();
        _selectedSkinView = skinView;
    }
}