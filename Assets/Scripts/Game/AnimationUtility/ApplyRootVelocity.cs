using UnityEngine;

public class ApplyRootVelocity : StateMachineBehaviour
{
    AnimationCtrl _animCtrl;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = true;

        if(!_animCtrl)_animCtrl = animator.gameObject.GetComponent<AnimationCtrl>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = true;
        Vector3 velo = new Vector3(animator.rootPosition.x,animator.transform.position.y, animator.rootPosition.z);
        _animCtrl.OwerPos.position = velo;
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = false;
    }
}
