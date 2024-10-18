using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : Popup
{
    private readonly float NewScale = 1f;
    private readonly float Duration = 1f;

    [SerializeField] private UpperPart _upperPart;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _rewardButton;

    private SaveLoader _saveLoader;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;

    private void OnEnable()
    {
        if (_upperPart == null)
        {
            throw new ArgumentNullException(nameof(_upperPart));
        }

        if (_nextButton == null)
        {
            throw new ArgumentNullException(nameof(_nextButton));
        }

        if (_rewardButton == null)
        {
            throw new ArgumentNullException(nameof(_rewardButton));
        }

        _upperPart.transform.localScale = Vector3.zero;

        _upperPart.transform.DOScale(NewScale, Duration)
            .SetEase(Ease.OutBounce);

        _nextButton.onClick.AddListener(Next);
        _rewardButton.onClick.AddListener(GetReward);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(Next);
        _rewardButton.onClick.AddListener(GetReward);
    }

    public void Init(SaveLoader saveLoader, InterstitialAd interstitialAd, RewardedAd rewardedAd)
    {
        _saveLoader = saveLoader;
        _interstitialAd = interstitialAd;
        _rewardedAd = rewardedAd;
        enabled = true;
    }

    private void Next()
    {
        _interstitialAd.Show();
    }

    private void GetReward()
    {
        _rewardedAd.Show((int)Reward.Resources);
        _rewardButton.interactable = false;
    }
}