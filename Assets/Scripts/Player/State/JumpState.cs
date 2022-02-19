using UnityEngine;

public partial class PlayerStateMachine : MonoBehaviour
{
    public class JumpState : PlayerStateBase
    {
       
        public override void OnEnter(PlayerStateMachine owner, PlayerStateBase prevState)
        {
            owner.PlayAnimation("Jump",0.1f,owner.m_currentAnimLayer);
            owner.m_currentVelocity.y = owner.m_jumpPower;
            owner.m_currentJumpStep -= 1;
        }

        public override void OnExit(PlayerStateMachine owner, PlayerStateBase nextState)
        {
            
        }

        public override void OnUpdate(PlayerStateMachine owner)
        {
            if (owner.IsGround())
            {
                owner.ChangeState(owner.m_landState);
            }
            if (owner.m_currentVelocity.y < 0f)
            {
                owner.ChangeState(owner.m_fallState);
            }
            if (owner.m_inputManager.AvoidKey == KeyStatus.DOWN)
            {
                owner.ChangeState(owner.m_avoidState);
            }
            if (owner.m_inputManager.AttackKey == KeyStatus.DOWN)
            {
                owner.ChangeState(owner.m_attackState);
            }
        }
    }
}
