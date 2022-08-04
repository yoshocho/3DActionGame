using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<PlayerStateMachine>.State;

partial class PlayerStateMachine : CharacterBase
{
    public class PlayerJumpState : State
    {
        bool _canJump = true;
        const float _waitTime = 0.3f;
        protected override void OnEnter(State prevState)
        {
            if(owner._debagMode) Debug.Log("Jump");

            owner.PlayAnimation("Jump",0.1f);
            owner._currentJumpCount++;
            if (owner._inputAxis.magnitude > 0.1f) owner._targetRot = Quaternion.LookRotation(owner._moveForward);
            owner._currentVelocity = new Vector3(owner._moveForward.x,owner._jumpPower,owner._moveForward.z);
            owner.StartCoroutine(WaitJump());
        }
        protected override void OnUpdate()
        {
            if (owner._inputAxis.sqrMagnitude > 0.1f) owner._targetRot = Quaternion.LookRotation(owner._moveForward);
            if (owner._inputProvider.GetAvoid()) owner.ChangeState(StateEvent.Avoid);
            if (owner._inputProvider.GetAttack()) owner.ChangeState(StateEvent.Attack);

            if (owner._inputProvider.GetJump() && _canJump && owner._currentJumpCount < owner._jumpCount)
                owner.ChangeState(StateEvent.Jump);
            
            if (owner._currentVelocity.y < 0.0f) owner.ChangeState(StateEvent.Fall);
        }
        protected override void OnExit(State nextState)
        {

        }
        IEnumerator WaitJump()
        {
            _canJump = false;
            yield return new WaitForSeconds(_waitTime);
            _canJump = true;
        }
    }
}