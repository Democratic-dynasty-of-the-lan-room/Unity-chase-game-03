using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.CustomInput
{
    public class OnLandInput : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; } = Vector2.zero;
        public bool MoveIsPressed { get; private set; } = false;
        public Vector2 LookInput { get; private set; } = Vector2.zero;

        private InputActions _input;
        
        private void OnEnable()
        {
            _input = new InputActions();
            _input.OnLand.Enable();

            _input.OnLand.Move.performed += SetMove;
            _input.OnLand.Move.canceled += SetMove;
            
            _input.OnLand.Look.performed += SetLook;
            _input.OnLand.Look.canceled += SetLook;
        }
        
        private void OnDisable()
        {
            _input.OnLand.Move.performed -= SetMove;
            _input.OnLand.Move.canceled -= SetMove;
            
            _input.OnLand.Look.performed -= SetLook;
            _input.OnLand.Look.canceled -= SetLook;
            
            _input.OnLand.Disable();
        }

        private void SetMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
            MoveIsPressed = MoveInput != Vector2.zero;
        }
        
        private void SetLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }
    }
}
