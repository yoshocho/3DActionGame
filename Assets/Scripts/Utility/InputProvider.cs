using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputProviders
{
    public class InputProvider : IInputProvider
    {
        PlayerInput _inputAction;

        public void SetUp(PlayerInput input)
        {
            _inputAction = input;
        }
       

        bool CheakInput(InputAction input,InputType type)
        {
            switch (type)
            {
                case InputType.None:
                    return false;
                case InputType.Down:
                    return input.WasPressedThisFrame();
                case InputType.Up:
                    return input.WasReleasedThisFrame();
                case InputType.Pressed:
                    return input.IsPressed();
                default:
                    return false;
            }
        }

        public bool GetJump(InputType type)
        {
            return CheakInput(_inputAction.Player.Jump, type);
        }

        public bool GetAttack(InputType type)
        {
            return CheakInput(_inputAction.Player.Attack,type);
        }

        public bool GetAvoid(InputType type)
        {
            return CheakInput(_inputAction.Player.Avoid, type);
        }
        public Vector3 GetInputDirection()
        {
            var axis = _inputAction.Player.Move.ReadValue<Vector2>();
            var inputDir = Vector3.forward * axis.y + Vector3.right * axis.x;
            return inputDir;
        }
        public Vector2 GetCameraAxis()
        {
            return _inputAction.Player.CameraAxis.ReadValue<Vector2>();
        }
        public IInputProvider SetCallBack(InputAction input, Action onCall)
        {
            input.started += context => onCall.Invoke();
            return this;
        }
    }
}