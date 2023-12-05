using UnityEngine;

public class Aim : MonoBehaviour
{
    private readonly float RotationX = 90;

    public void RotateTo(Quaternion newRotation)
    {
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.x = RotationX;
        transform.eulerAngles = eulerAngles;
    }
}