using System;
using UnityEngine;

public class ColliderEventHandler : MonoBehaviour
{
    public event Action<Collision> Collided;

    private void OnCollisionEnter(Collision collision)
    {
        Collided?.Invoke(collision);
    }
}