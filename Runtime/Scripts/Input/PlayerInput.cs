using UnityEngine;

namespace InexperiencedDeveloper.Controllers.Input
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputActions { get; private set; }

        public void Init()
        {
            InputActions = new PlayerInputActions();
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }

        public Vector2 Move => InputActions.Player.Movement.ReadValue<Vector2>();
        public Vector2 Look => InputActions.Player.Look.ReadValue<Vector2>();
        public bool Sprint => InputActions.Player.Sprint.IsPressed();
        public bool Jump => InputActions.Player.Jump.WasPressedThisFrame();
    }
}

