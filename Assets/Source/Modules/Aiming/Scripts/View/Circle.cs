namespace Aiming
{
    public class Circle : AimElement
    {
        private ICatchable[] _projectiles;

        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (ICatchable projectile in _projectiles)
            {
                projectile.Catched += IncreaseScale;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (ICatchable projectile in _projectiles)
            {
                projectile.Catched -= IncreaseScale;
            }
        }

        public void Init(ICatchable[] projectiles)
        {
            _projectiles = projectiles;
        }
    }
}