using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnEnable()
    {

    }

    public void Init()
    {
        enabled = true;
    }

    public void OnThrow(Transform newParent)
    {
        transform.parent = newParent;
    }
}