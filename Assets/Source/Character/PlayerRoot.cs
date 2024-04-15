public class PlayerRoot : CharacterRoot
{
    private PlayerInputRouter _inputRouter;
    private YandexLeaderboard _leaderboard;

    protected override void Start()
    {
        PlayerTargeting targeting = (PlayerTargeting)Aiming;
        targeting.Init(_inputRouter);

        Player player = (Player)Person;
        _leaderboard.Init(player);

        base.Start();
    }

    public void Init(PlayerInputRouter inputRouter, YandexLeaderboard leaderboard)
    {
        _inputRouter = inputRouter;
        _leaderboard = leaderboard;
    }
}