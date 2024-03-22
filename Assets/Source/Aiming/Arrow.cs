using UnityEngine;

public class Arrow : AimElement
{
    private readonly float RotationX = 90;

    public void RotateTo(Quaternion newRotation)
    {
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.x = RotationX;
        transform.eulerAngles = eulerAngles;
    }
}