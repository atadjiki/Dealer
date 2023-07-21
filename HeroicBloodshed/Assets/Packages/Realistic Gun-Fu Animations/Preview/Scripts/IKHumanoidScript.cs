using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHumanoidScript : StateMachineBehaviour
{    
    public Transform objToHold;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        objToHold = animator.GetComponent<AnimationPreviewer>().ikOBJToHold;

        // Set the right hand target position and rotation, if one has been assigned 
        if(objToHold != null) 
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1); 
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1); 
            animator.SetIKPosition(AvatarIKGoal.RightHand,objToHold.position); 
            animator.SetIKRotation(AvatarIKGoal.RightHand,objToHold.rotation); 
        }  
    }
}
