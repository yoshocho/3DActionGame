using UnityEngine;

public partial class PlayerStateMachine : MonoBehaviour
{
    public class FallState : PlayerStateBase
    {
        public override void OnEnter(PlayerStateMachine owner, PlayerStateBase prevState)
        {
            owner.m_currentGravityScale = owner.m_gravityScale;
            owner.PlayAnimation("Fall",0.1f);
            Debug.Log("InFall");
        }

        public override void OnExit(PlayerStateMachine owner, PlayerStateBase nextState)
        {

        }

        public override void OnUpdate(PlayerStateMachine owner)
        {
            //owner.m_currentVelocity = new Vector3(owner.m_inputManager.InputDir.x * owner.m_jumpMoveSpeed
            //    , Physics.gravity.y * owner.m_gravityScale,
            //    owner.m_inputDir.z * owner.m_jumpMoveSpeed);

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

