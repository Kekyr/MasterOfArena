using System;
using UnityEngine;

namespace CubeFeature
{
    public class CubeExplosion : MonoBehaviour
    {
        public event Action Stopped;

        private void OnParticleSystemStopped()
        {
            Stopped?.Invoke();
        }
    }
}