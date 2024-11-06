using System;
using UnityEngine;

namespace BombPlatformFeature
{
    public interface IAttacker
    {
        public event Action<Vector3, uint> Attacked;
    }
}