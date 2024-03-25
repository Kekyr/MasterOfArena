public class PlayerRoot : CharacterRoot
{
    private PlayerInputRouter _inputRouter;

    protected override void Start()
    {
        PlayerTargeting targeting = (PlayerTargeting)Aiming;
        targeting.Init(_inputRouter);
        base.Start();
    }

    public void Init(PlayerInputRouter inputRouter)
    {
        _inputRouter = inputRouter;
    }
}