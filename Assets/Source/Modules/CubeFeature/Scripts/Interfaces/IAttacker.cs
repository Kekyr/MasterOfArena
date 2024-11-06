using UnityEngine;

namespace CubeFeature
{
    public interface IAttacker
    {
        public Color DamageMarkColor { get; }

        public void Attack(Cube cube);
    }
}