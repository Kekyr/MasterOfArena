using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : Popup
{
    private readonly float _newScale = 1f;
    private readonly float _duration = 1f;

    [SerializeField] private Transform _upperPart;
    [SerializeField] private Button _rewardButton;

    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;

    protected override void OnEnable()
    {
        if (_upperPart == null)
        {
            throw new ArgumentNullException(nameof(_upperPart));
        }

        if (_rewardButton == null)
        {
            throw new ArgumentNullException(nameof(_rewardButton));
        }

        _upperPart.localScale = Vector3.zero;

        _upperPart.DOScale(_newScale, _duration)
            .SetEase(Ease.OutBounce);

        _rewardButton.onClick.AddListener(GetReward);
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        _rewardButton.onClick.AddListener(GetReward);
        base.OnDisable();
    }

    public void Init(InterstitialAd interstitialAd, RewardedAd rewardedAd)
    {
        _interstitialAd = interstitialAd;
        _rewardedAd = rewardedAd;
        enabled = true;
    }

    protected override void Next()
    {
        _interstitialAd.Show();
    }

    private void GetReward()
    {
        _rewardedAd.Show((int)Reward.Resources);
        _rewardButton.interactable = false;
    }
}