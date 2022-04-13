using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class LandState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner.PlayAnimation("Land",0.1f);
            owner.m_currentJumpStep = owner.m_jumpStep;
            owner.m_currentAirDushCount = 0;
            owner.m_lunchEnd = false;
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
        }

        public override void OnUpdate(Player owner)
        {
            if (owner.m_inputDir.sqrMagnitude > 0.1f)
            {
                Debug.Log("Land -> Walk");
                owner.ChangeState(owner.m_walkState);
            }
            else if(!owner.m_animCtrl.IsPlayingAnimatin())
            {
                owner.m_currentVelocity.x = 0f;
                owner.m_currentVelocity.z = 0f;
                Debug.Log("Land -> Idle");
                owner.ChangeState(owner.m_idleState);
            }
            else
            {
                owner.m_currentVelocity.x = 0f;
                owner.m_currentVelocity.z = 0f;
            }
            if (owner.m_inputManager.AvoidKey == KeyStatus.DOWN)
            {
                Debug.Log("Land -> Jump");
                owner.ChangeState(owner.m_avoidState);
            }
            if (owner.m_inputManager.LunchKey is KeyStatus.STAY) owner.m_lunchAttack = true;
            else owner.m_lunchAttack = false;
            if (owner.m_inputManager.AttackKey == KeyStatus.DOWN)
            {
                owner.ChangeState(owner.m_attackState);
            }

            if (owner.m_inputManager.JumpKey == KeyStatus.DOWN)
            {
                owner.ChangeState(owner.m_jumpState);
            }
        }
    }
}