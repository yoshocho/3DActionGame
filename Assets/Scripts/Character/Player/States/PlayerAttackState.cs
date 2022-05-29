using AttackSetting;
using UnityEngine;
using State = StateMachine<NewPlayer>.State;

public partial class NewPlayer : CharacterBase
{
    public class PlayerAttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            owner._currentVelocity = Vector3.zero;
            owner._actionCtrl.RequestAction(AttackType.Weak);
        }
        protected override void OnUpdate()
        {
            if (owner._inputProvider.GetAttack() && !owner._actionCtrl.ActionKeep)
            {
                if (owner.IsGround()) owner._actionCtrl.RequestAction(AttackType.Weak);
                else owner._actionCtrl.RequestAction(AttackType.Airial);
            }

            if ((owner.IsGround()))
            {
                if (owner._inputAxis.sqrMagnitude > 0.1f && 
                    owner._actionCtrl.ReceiveTimer < owner._actionCtrl.CurrentAction.ReceiveTime - 0.1f)
                    owner.ChangeState(StateEvent.Walk);
                else if (!owner._animCtrl.IsPlayingAnimatin()) owner.ChangeState(StateEvent.Idle);
            }
            else if (!owner._actionCtrl.ActionKeep) owner.ChangeState(StateEvent.Fall);

        }
        protected override void OnExit(State nextState)
        {
            owner._actionCtrl.EndAttack();
        }
    }
}