using System;
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

        public bool GetAvoidDown()
        {
            return _inputAction.Player.Avoid.IsPressed();
        }

        public Vector2 GetCameraAxis()
        {
            return _inputAction.Player.CameraAxis.ReadValue<Vector2>();
        }

        public IInputProvider SetCallBack(UnityEngine.InputSystem.InputAction input, Action onCall)
        {
            input.started += context => onCall.Invoke();
            return this;
        }
    }
}