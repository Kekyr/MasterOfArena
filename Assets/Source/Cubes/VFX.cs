using System;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public event Action Stopped;

    private void OnParticleSystemStopped()
    {
        Stopped?.Invoke();
    }
}