using UnityEngine;

namespace Aiming
{
    public class Arrow : AimElement
    {
        private readonly float _rotationX = 90;

        public void RotateTo(Quaternion newRotation)
        {
            Vector3 eulerAngles = newRotation.eulerAngles;
            eulerAngles.x = _rotationX;
            transform.eulerAngles = eulerAngles;
        }
    }
}