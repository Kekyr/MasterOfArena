using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _force;

    public void Shake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(_force);
    }
}