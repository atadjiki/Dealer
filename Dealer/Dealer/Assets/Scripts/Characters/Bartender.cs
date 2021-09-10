using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bartender : NPCController
{
    public override IEnumerator DoConversation()
    {
        yield return base.DoConversation();

        if(GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == false)
        {
            UIManager.Instance.DisplaySpeech("bartender: those guys at the corner table want to speak with you.", 3.5f);
        }
        else
        {
            UIManager.Instance.DisplaySpeech("bartender: almost off my shift. I hate this dump.", 3.5f);
        }

       
        PlayerController.Instance.ToIdle();
    }
}
