using UnityEngine;

public class AIRoot : CharacterRoot
{
    private CubeSpawner _cubeSpawner;

    protected override void Start()
    {
        Debug.Log(Aiming.GetType().FullName);
        AiTargeting targeting = (AiTargeting)Aiming;
        targeting.Init(_cubeSpawner, Projectiles);
        base.Start();
    }

    public void Init(CubeSpawner cubeSpawner)
    {
        _cubeSpawner = cubeSpawner;
    }
}