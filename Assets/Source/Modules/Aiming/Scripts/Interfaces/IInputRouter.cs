using UnityEngine.InputSystem;

namespace Aiming
{
    public interface IInputRouter
    {
        public InputAction Aiming { get; }
    }
}