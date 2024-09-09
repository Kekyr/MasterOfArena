public class SkinRewardedAd : RewardedAd
{
    private SkinView _currentSkinView;
    private Music _music;

    public void Init(Music music)
    {
        _music = music;
    }

    public void Show(SkinView skinView)
    {
        _currentSkinView = skinView;
        Show();
    }

    public override void Show()
    {
        Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardCallback, OnCloseCallback);
    }

    protected override void OnOpenCallback()
    {
        _music.Pause();
    }

    protected override void OnRewardCallback()
    {
        _currentSkinView.OnBuySuccess();
        base.OnRewardCallback();
    }

    protected override void OnCloseCallback()
    {
        _music.Continue();
    }
}