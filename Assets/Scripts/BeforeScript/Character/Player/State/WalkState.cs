using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class WalkState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner._moveSpeed = owner._walkSpeed;
            owner.PlayAnimation("Walk" ,0.2f);
        }
        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
        }
        public override void OnUpdate(Player owner)
        {
            if (owner.IsGround())
            {
                if (owner._inputDir.sqrMagnitude > 0.1f)
                {
                    owner._targetRot = Quaternion.LookRotation(owner._moveForward);
                    owner._currentVelocity = new Vector3(owner._moveForward.x, owner._currentVelocity.y , owner._moveForward.z);
                    //owner.m_currentVelocity =
                    //    new Vector3(owner.m_selfTrans.forward.x, owner.m_currentVelocity.y, owner.m_selfTrans.forward.z);
                    if(owner._inputProvider.GetAvoid())
                    {
                        owner.ChangeState(owner._avoidState);
                    }
                    if (owner._inputProvider.GetAttack())
                    {
                        owner.ChangeState(owner._attackState);
                    }
                    if (owner._inputProvider.GetJump() && owner._jumpStep >= 0)
                    {
                        owner.ChangeState(owner._jumpState);
                    }
                }
                else
                {
                    owner.ChangeState(owner._idleState);
                }
            }
            else
            {
                owner.ChangeState(owner._fallState);
            }
        }
    }
}