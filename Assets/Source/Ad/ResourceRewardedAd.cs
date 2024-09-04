using System;

public class ResourceRewardedAd : RewardedAd
{
    private PlayerDataSO _playerData;
    private SaveLoader _saveLoader;
    private Coins _coins;

    public event Action<int> Rewarded;

    public void Init(PlayerDataSO playerData, SaveLoader saveLoader, Coins coins)
    {
        if (playerData == null)
        {
            throw new ArgumentNullException(nameof(playerData));
        }

        if (saveLoader == null)
        {
            throw new ArgumentNullException(nameof(saveLoader));
        }

        if (coins == null)
        {
            throw new ArgumentNullException(nameof(coins));
        }

        _playerData = playerData;
        _saveLoader = saveLoader;
        _coins = coins;
    }

    protected override void OnRewardCallback()
    {
        _coins.Add();
        _playerData.AddScore();
        Rewarded?.Invoke(_playerData.Score);
        _saveLoader.Save();
    }
}