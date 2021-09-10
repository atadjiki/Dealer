using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkGuy : NPCController
{
    private int maxConvos = 2;
    private int currentConvo = 0;

    public override IEnumerator DoConversation()
    {
        yield return base.DoConversation();

        if (currentConvo == 0)
        {
            UIManager.Instance.DisplaySpeech("drunk guy: i'm seeing double...", 3.5f);
        }
        else if (currentConvo == 1)
        {
            UIManager.Instance.DisplaySpeech("drunk guy: hey, i'll buy you a drink. just joking.", 3.5f);
        }
        else if (currentConvo == 2)
        {
            UIManager.Instance.DisplaySpeech("drunk guy: i lost my job...you hiring? *burp*", 3.5f);
        }

        currentConvo++;
        if(currentConvo > maxConvos) { currentConvo = 0; }
        PlayerController.Instance.ToIdle();

    }
}
