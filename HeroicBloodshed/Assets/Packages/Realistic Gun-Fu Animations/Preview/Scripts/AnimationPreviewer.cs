using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AnimationPreviewer : MonoBehaviour
{
    Animator anim;
    private Vector3 oldLoc;
    private Quaternion oldRot;
    public Transform ikOBJToHold, a, b;
    float t;

    void Start() 
    {
        anim = GetComponent<Animator>();
        oldLoc = transform.position;
        oldRot = transform.rotation;
    }

    void Update () 
    {
        // Check Animator clip's tag and switch animator bool
        // (animator tag is found in mecanim in the animation clip's components)
        if (gameObject.tag == "Player") 
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag ("JG1")) 
            {
                anim.SetBool ("InJudoGrab1", true);
            } 
            else 
            {
                anim.SetBool ("InJudoGrab1", false);
            }

            // Also increment and decrement float t here for the lerp below
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag ("JG2")) 
            {   
                 if (t < 1) {t += Time.deltaTime;}

                anim.SetBool ("InJudoGrab2", true);
            } 
            else 
            {
                if (t > 0) {t -= Time.deltaTime;}

                anim.SetBool ("InJudoGrab2", false);
            }   
        }     

        // Lerp code for the rotation of the IK Object from object A to B or back (that the Enemy's hand is attached to)
        ikOBJToHold.transform.rotation = Quaternion.Lerp (a.rotation, b.rotation, t); 

        // Reset all the triggers in mecanim 
        if (Input.GetMouseButtonUp(0)) 
        {
          Invoke ("ResetTriggers", 0.5f);
        }
    }

    public void ResetTriggers () 
    {
        anim.ResetTrigger ("Melee");
        anim.ResetTrigger ("Aim");
        anim.ResetTrigger ("Execute");
        anim.ResetTrigger ("Reset");
    }
    // Reset the position and rotation of both characters
    public void ResetButton () 
    {
        transform.position = oldLoc;
        transform.rotation = oldRot;
    }
}