using UnityEngine;
using State = StateMachine<PlayerStateMachine>.State;

public partial class PlayerStateMachine : CharacterBase
{
    public class PlayerAvoidState : State
    {
        Vector3 _avoidAxis = Vector3.zero;
        float _avoidTimer = 0.0f;
        float _justTimer = 0.0f;
        protected override void OnEnter(State prevState)
        {
            if(owner._debagMode)Debug.Log("InAvoid");

            _avoidAxis = owner._moveForward.normalized;
            if (owner.IsGround()) owner.PlayAnimation("Avoid");
            else owner.PlayAnimation("AirDush");

            owner._inAvoid = true;
            _avoidTimer = owner._avoidTime;
            _justTimer = owner._justTime;
        }
        protected override void OnUpdate()
        {
            _avoidTimer -= Time.deltaTime;
            _justTimer -= Time.deltaTime;
            if (_justTimer < 0.0f)
            {
                owner._inAvoid = false;
                _justTimer = 0.0f;
            }

            if (_avoidTimer > 0.0f)
            {
                if (owner._inputAxis.sqrMagnitude < 0.05) _avoidAxis = owner._selfTrans.forward * owner._avoidSpeed;
                _avoidAxis.y = 0.0f;
                owner._targetRot = Quaternion.LookRotation(_avoidAxis);
                owner._currentVelocity = _avoidAxis * owner._avoidSpeed;
            }
            else
            {
                if (owner.IsGround())
                {
                    if (owner._inputAxis.sqrMagnitude > 0.1f)
                    {
                        if (owner._inputProvider.GetAvoidDown()) owner.ChangeState(StateEvent.Sprint);
                        else owner.ChangeState(StateEvent.Run);
                    }
                    else owner.ChangeState(StateEvent.Idle);
                    if (owner._inputProvider.GetJump() && owner._currentJumpCount < owner._jumpCount) 
                        owner.ChangeState(StateEvent.Jump);
                }
                else owner.ChangeState(StateEvent.Fall);
            }
        }
        protected override void OnExit(State nextState)
        {
            owner._inAvoid = false;
            _avoidTimer = 0.0f;
            _justTimer = 0.0f;
        }
    }
}