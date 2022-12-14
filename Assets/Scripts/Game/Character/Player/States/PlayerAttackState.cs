using AttackSetting;
using UnityEngine;
using State = StateMachine<PlayerStateMachine>.State;

public partial class PlayerStateMachine : CharacterBase
{
    public class PlayerAttackState : State
    {
        
        const float _extendTime = 0.25f;
        protected override void OnEnter(State prevState)
        {

            //owner._currentVelocity = Vector3.zero;
            if (owner._debagMode) Debug.Log("InAttackState");
            owner._mover.Velocity = new Vector3(0.0f,owner._mover.Velocity.y,0.0f);
            owner._currentStyle = StyleState.Strafe;
            owner._playerActCtrl.SetStyle(owner._currentWeapon);
            owner._actionCtrl.RequestAction(owner._attackType);
            AttackAssist();
        }
        protected override void OnUpdate()
        {
            bool stickMove = owner._inputAxis.sqrMagnitude > 0.1f;
            if (owner._inputProvider.GetAttack(InputType.Down) && !owner._actionCtrl.IsActionKeep())
            {
                if (owner.IsGround()) owner._actionCtrl.RequestAction(owner._attackType);
                else owner._actionCtrl.RequestAction(AttackType.Airial);
                AttackAssist();
            }

            if (owner.IsGround())
            {
                if (stickMove && owner._actionCtrl.ReceiveTimer <
                    owner._actionCtrl.CurrentAction.ReceiveTime - _extendTime) //’¼‚®‚ÉƒXƒe[ƒg‚ª•Ï‚í‚ç‚È‚¢‚æ‚¤‚É­‚µ—P—\‚ðŽ‚½‚¹‚é
                    owner.ChangeState(StateEvent.Run);
                else if (!owner._animCtrl.IsPlayingAnimatin()) owner.ChangeState(StateEvent.Idle);
            }
            else if (!owner._actionCtrl.IsActionKeep()) owner.ChangeState(StateEvent.Fall);

            if (owner._inputProvider.GetAvoid(InputType.Down)) owner.ChangeState(StateEvent.Avoid);
            if (owner._inputProvider.GetJump(InputType.Down) && owner._currentJumpCount < owner._jumpCount)
                owner.ChangeState(StateEvent.Jump);
        }
        protected override void OnExit(State nextState)
        {
            owner._actionCtrl.EndAttack();
        }
        void AttackAssist()
        {
            Vector3 dir = owner._mover.Velocity;
            if(GameManager.Instance.LockOnTarget != null)
            {
                dir = GameManager.Instance.LockOnTarget.position - owner._selfTrans.position;
            }
            else
            {
                if (owner._inputAxis.sqrMagnitude > 0.1f)
                    dir = owner._moveForward;
            }
            owner.DoRotate(dir);
        }
    }
}