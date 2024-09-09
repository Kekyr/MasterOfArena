using System;
using UnityEngine;

[RequireComponent(typeof(SFX))]
public abstract class RewardedAd : MonoBehaviour
{
    [SerializeField] private SFXSO _success;

    private SaveLoader _saveLoader;
    private SFX _sfx;

    private void OnEnable()
    {
        if (_success == null)
        {
            throw new ArgumentNullException(nameof(_success));
        }

        _sfx = GetComponent<SFX>();
    }

    public void Init(SaveLoader saveLoader)
    {
        _saveLoader = saveLoader;
    }

    public virtual void Show()
    {
        Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardCallback, OnCloseCallback);
    }

    protected virtual void OnRewardCallback()
    {
        _sfx.Play(_success);
        _saveLoader.Save();
    }

    protected virtual void OnOpenCallback()
    {
        Time.timeScale = 0;
    }

    protected virtual void OnCloseCallback()
    {
        Time.timeScale = 1;
    }
}