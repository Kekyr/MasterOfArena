using System;
using UnityEngine;

public class CubeExplosion : MonoBehaviour
{
    public event Action Stopped;

    private void OnParticleSystemStopped()
    {
        Stopped?.Invoke();
    }
}