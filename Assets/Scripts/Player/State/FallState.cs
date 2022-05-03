using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class FallState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner.m_currentGravityScale = owner.m_gravityScale;
            owner.PlayAnimation("Fall",0.1f);
            Debug.Log("InFall");
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {

        }

        public override void OnUpdate(Player owner)
        {
           
            if (owner.m_inputManager.AttackKey == KeyStatus.DOWN && !owner.m_airAttackEnd)
            {
                owner.ChangeState(owner.m_attackState);
            }
            if (owner.IsGround())
            {
                owner.ChangeState(owner.m_landState);
            }
            if (owner.m_inputManager.AvoidKey == KeyStatus.DOWN && owner.m_currentAirDushCount < owner.m_airDushCount)
            {
                owner.ChangeState(owner.m_avoidState);
            }
            if (owner.m_inputManager.JumpKey == KeyStatus.DOWN && owner.m_currentJumpStep > 0)
            {
                owner.ChangeState(owner.m_jumpState);
            }
        }
    }
}

