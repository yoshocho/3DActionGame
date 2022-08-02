using AttackSetting;
using UnityEngine;
using State = StateMachine<NewPlayer>.State;

public partial class NewPlayer : CharacterBase
{
    public class PlayerAttackState : State
    {
        AttackType _type = AttackType.Heavy;
        const float _extendTime = 0.25f;
        protected override void OnEnter(State prevState)
        {
            owner._currentVelocity = Vector3.zero;
            owner._actionCtrl.RequestAction(_type);
            AttackAssist();
        }
        protected override void OnUpdate()
        {
            bool stickMove = owner._inputAxis.sqrMagnitude > 0.1f;
            if (owner._inputProvider.GetAttack() && !owner._actionCtrl.ActionKeep)
            {
                if (owner.IsGround()) owner._actionCtrl.RequestAction(_type);
                else owner._actionCtrl.RequestAction(AttackType.Airial);
                AttackAssist();
                #region
                //if (GameManager.Instance.LockOnTarget == null)
                //{
                //    if (stickMove)
                //        owner._targetRot = Quaternion.LookRotation(owner._moveForward);
                //}
                //else
                //{
                //    AttackAssist(GameManager.Instance.LockOnTarget.transform.position);
                //}
                #endregion
            }



            if (owner.IsGround())
            {
                if (stickMove && owner._actionCtrl.ReceiveTimer <
                    owner._actionCtrl.CurrentAction.ReceiveTime - _extendTime) //’¼‚®‚ÉƒXƒe[ƒg‚ª•Ï‚í‚ç‚È‚¢‚æ‚¤‚É­‚µ—P—\‚ðŽ‚½‚¹‚é
                    owner.ChangeState(StateEvent.Walk);
                else if (!owner._animCtrl.IsPlayingAnimatin()) owner.ChangeState(StateEvent.Idle);
            }
            else if (!owner._actionCtrl.ActionKeep) owner.ChangeState(StateEvent.Fall);

            if (owner._inputProvider.GetAvoid()) owner.ChangeState(StateEvent.Avoid);
            if (owner._inputProvider.GetJump() && owner._currentJumpCount < owner._jumpCount)
                owner.ChangeState(StateEvent.Jump);
        }
        protected override void OnExit(State nextState)
        {
            owner._actionCtrl.EndAttack();
        }
        void AttackAssist()
        {
            Vector3 dir = owner._currentVelocity;
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