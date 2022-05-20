using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class JumpState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner.PlayAnimation("Jump",0.1f);
            owner._moveForward.y = 0;
            if (owner._inputDir.sqrMagnitude > 0.1) owner._targetRot = Quaternion.LookRotation(owner._moveForward);
            owner._currentVelocity = new Vector3(owner._moveForward.x, owner._jumpPower, owner._moveForward.z);
            owner._currentJumpStep -= 1;
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
            
        }

        public override void OnUpdate(Player owner)
        {
            //if (owner.IsGround())
            //{
            //    owner.ChangeState(owner.m_landState);
            //}
            if (owner._currentVelocity.y < 0f)
            {
                owner.ChangeState(owner._fallState);
            }
            if (owner._inputProvider.GetAvoid() && owner._currentAirDushCount < owner._airDushCount)
            {
                owner.ChangeState(owner._avoidState);
            }
            if (owner._inputProvider.GetAttack())
            {
                owner.ChangeState(owner._attackState);
            }
        }
    }
}
