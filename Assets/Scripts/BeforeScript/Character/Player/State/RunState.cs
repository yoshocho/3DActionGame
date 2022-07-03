using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class RunState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner._moveSpeed = owner._dashSpeed;
            owner.PlayAnimation("Run",0.3f);
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
            owner._moveSpeed = owner._walkSpeed;    
        }

        public override void OnUpdate(Player owner)
        {
            if (owner.IsGround())
            {
                if (owner._inputDir.sqrMagnitude > 0.1f)
                {
                    var dir = owner._moveForward;
                    dir.y = 0f;
                    owner._targetRot = Quaternion.LookRotation(dir);
                    owner._currentVelocity =
                      new Vector3(owner._selfTrans.forward.x, owner._currentVelocity.y, owner._selfTrans.forward.z);
                }
                else
                {
                    owner.ChangeState(owner._idleState);
                }
                if(owner._inputProvider.GetAvoid())
                {
                    owner.ChangeState(owner._avoidState);
                }
                if (owner._inputProvider.GetAttack())
                {
                    owner.ChangeState(owner._attackState);
                }
                if(owner._inputProvider.GetJump() && owner._currentJumpStep >= 0)
                {
                    owner.ChangeState(owner._jumpState);
                }
            }
            else
            {
                owner.ChangeState(owner._fallState);
            }
        }
    }
}
