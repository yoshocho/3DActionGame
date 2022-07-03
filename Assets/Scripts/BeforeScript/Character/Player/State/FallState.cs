using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class FallState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner._currentGravityScale = owner._gravityScale;
            owner.PlayAnimation("Fall",0.1f);
            Debug.Log("InFall");
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {

        }

        public override void OnUpdate(Player owner)
        {
           
            if (owner._inputProvider.GetAttack() && !owner._airAttackEnd)
            {
                owner.ChangeState(owner._attackState);
            }
            if (owner.IsGround())
            {
                owner.ChangeState(owner._landState);
            }
            if (owner._inputProvider.GetAvoid() && owner._currentAirDushCount < owner._airDushCount)
            {
                owner.ChangeState(owner._avoidState);
            }
            if (owner._inputProvider.GetJump() && owner._currentJumpStep > 0)
            {
                owner.ChangeState(owner._jumpState);
            }
        }
    }
}

