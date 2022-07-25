using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
   
    public class AttackState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner._currentVelocity.x = 0f;
            owner._currentVelocity.z = 0f;
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
            if (nextState is not IdleState)
            {
                owner._weaponHolder.ResetHolder();
                owner.m_poseKeep = false;
            }
        }

        public override void OnUpdate(Player owner)
        {
          
            if (owner._inputProvider.GetAvoid() && !owner._stateKeep)
            {
                owner.ChangeState(owner._avoidState);
            }
            if (owner._inputProvider.GetJump() && owner._currentJumpStep >= 0)
            {
                owner.ChangeState(owner._jumpState);
            }
        }
    }
}
