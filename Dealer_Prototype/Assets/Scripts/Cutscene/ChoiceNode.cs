using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceNode : CutsceneNode
{
    public override void OnActionComplete()
    {
        foreach(CutsceneAction action in _actions)
        {
            if(action is ChoiceAction)
            {
                _next = action.
            }
        }
    }
}
