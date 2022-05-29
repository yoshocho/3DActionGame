using UnityEngine;

namespace InputProviders
{
    public class InputProvider : IInputProvider
    {
        PlayerInput _inputAction;

        public void SetUp(PlayerInput input)
        {
            _inputAction = input;
        }
        public bool GetAttack()
        {
            return _inputAction.Player.Attack.WasPressedThisFrame();
        }

        public bool GetAvoid()
        {
            return _inputAction.Player.Avoid.WasPressedThisFrame();
        }

        public bool GetJump()
        {
            return _inputAction.Player.Jump.WasPressedThisFrame();
        }

        public Vector3 GetInputDirection()
        {
            var axis = _inputAction.Player.Move.ReadValue<Vector2>();
            var inputDir = Vector3.forward * axis.y + Vector3.right * axis.x;
            return inputDir;
        }
    }
}