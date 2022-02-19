using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateMachine : MonoBehaviour
{
    public abstract class PlayerStateBase
    {
        public virtual void OnEnter(PlayerStateMachine owner,PlayerStateBase prevState) { }
        public virtual void OnUpdate(PlayerStateMachine owner) { }
        public virtual void OnExit(PlayerStateMachine owner,PlayerStateBase nextState) { }
    }
}
