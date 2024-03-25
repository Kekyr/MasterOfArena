public class AIRoot : CharacterRoot
{
    private CubeSpawner _cubeSpawner;

    protected override void Start()
    {
        AiTargeting targeting = (AiTargeting)Aiming;
        targeting.Init(_cubeSpawner, Projectiles);
        base.Start();
    }

    public void Init(CubeSpawner cubeSpawner)
    {
        _cubeSpawner = cubeSpawner;
    }
}