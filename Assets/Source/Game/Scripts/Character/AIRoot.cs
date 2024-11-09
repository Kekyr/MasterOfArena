using Aiming;
using Game;

namespace CharacterBase
{
    public class AIRoot : CharacterRoot
    {
        private CubeSpawner _cubeSpawner;

        protected override void Start()
        {
            AITargeting targeting = (AITargeting)Aiming;
            targeting.Init(_cubeSpawner, Projectiles);
            base.Start();
        }

        public void Init(CubeSpawner cubeSpawner)
        {
            _cubeSpawner = cubeSpawner;
        }
    }
}