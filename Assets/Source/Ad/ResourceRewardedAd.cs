using System;

public class ResourceRewardedAd : RewardedAd
{
    private PlayerDataSO _playerData;
    private Coins _coins;

    public event Action<int> Rewarded;

    public void Init(PlayerDataSO playerData, Coins coins)
    {
        _playerData = playerData;
        _coins = coins;
    }

    protected override void OnRewardCallback()
    {
        _coins.Increase();
        _playerData.AddScore();
        Rewarded?.Invoke(_playerData.Score);
        base.OnRewardCallback();
    }
}