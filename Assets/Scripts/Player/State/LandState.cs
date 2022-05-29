using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class LandState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner.PlayAnimation("Land",0.1f);
            owner._currentJumpStep = owner._jumpStep;
            owner._currentAirDushCount = 0;
            owner.m_lunchEnd = false;
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
        }

        public override void OnUpdate(Player owner)
        {
            if (owner._inputDir.sqrMagnitude > 0.1f)
            {
                Debug.Log("Land -> Walk");
                owner.ChangeState(owner._walkState);
            }
            else if(!owner._animCtrl.IsPlayingAnimatin())
            {
                owner._currentVelocity.x = 0f;
                owner._currentVelocity.z = 0f;
                Debug.Log("Land -> Idle");
                owner.ChangeState(owner._idleState);
            }
            else
            {
                owner._currentVelocity.x = 0f;
                owner._currentVelocity.z = 0f;
            }
            if (owner._inputProvider.GetAvoid())
            {
                Debug.Log("Land -> Jump");
                owner.ChangeState(owner._avoidState);
            }
            if (owner._inputProvider.GetAttack())
            {
                owner.ChangeState(owner._attackState);
            }

            if (owner._inputProvider.GetJump())
            {
                owner.ChangeState(owner._jumpState);
            }
        }
    }
}