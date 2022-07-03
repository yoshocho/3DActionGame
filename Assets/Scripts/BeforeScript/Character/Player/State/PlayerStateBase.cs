using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public abstract class PlayerStateBase
    {
        public virtual void OnEnter(Player owner,PlayerStateBase prevState) { }
        public virtual void OnUpdate(Player owner) { }
        public virtual void OnExit(Player owner,PlayerStateBase nextState) { }
    }
}
