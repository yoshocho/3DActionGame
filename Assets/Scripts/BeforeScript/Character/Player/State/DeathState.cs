using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class DeathState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner.PlayAnimation("Death");
            
            SceneChanger.Instance.FadeScene("TitleScene");
        }

        public override void OnUpdate(Player owner)
        {

        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {

        }
    }
}