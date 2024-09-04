using UnityEngine;

public class SkinRewardedAd : RewardedAd
{
    private SkinView _currentSkinView;

    public void Show(SkinView skinView)
    {
        _currentSkinView = skinView;
        Show();
    }

    protected override void OnRewardCallback()
    {
        _currentSkinView.OnBuySuccess();
    }
}
