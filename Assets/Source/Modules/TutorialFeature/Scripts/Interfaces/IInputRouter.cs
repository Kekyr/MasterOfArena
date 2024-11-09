using UnityEngine.InputSystem;

namespace TutorialFeature
{
    public interface IInputRouter
    {
        public InputAction Aiming { get; }
    }
}