using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiltyGuy : NPCController
{
    private bool complete = false;

    public override IEnumerator DoConversation()
    {
        yield return base.DoConversation();

        if(complete)
        {
            UIManager.Instance.DisplaySpeech("bouncer: thank you again. seriously.", 3.5f);
        }
        else
        {
            PlayerController.Instance.transform.LookAt(this.transform);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_OverShoulder);
            UIManager.Instance.DisplaySpeech("bouncer: sorry pal, private card game. come back later.", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_AtPlayer);
            UIManager.Instance.DisplaySpeech("you: maybe you can help me, i'm here on behalf on big jerome", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_OverShoulder);
            UIManager.Instance.DisplaySpeech("bouncer: big jerome? oh shit.", 3.5f);
            yield return new WaitForSeconds(3.5f);
            UIManager.Instance.DisplaySpeech("bouncer: look, he's trying to find who stole the money right?", 3.5f);
            yield return new WaitForSeconds(3.5f);
            UIManager.Instance.DisplaySpeech("bouncer: it was me. you gotta understand man, i needed the money.", 3.5f);
            yield return new WaitForSeconds(3.5f);
            UIManager.Instance.DisplaySpeech("bouncer: times are just tough, you know? look, please, please, help me out here.", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_AtPlayer);
            UIManager.Instance.DisplaySpeech("you: okay, okay, slow down one second.", 3.5f);
            yield return new WaitForSeconds(3.5f);
            UIManager.Instance.DisplaySpeech("you: i knew there'd be something.", 3.5f);
            yield return new WaitForSeconds(3.5f);
            UIManager.Instance.DisplaySpeech("you: look, just lay low and i'll say you skipped town.", 3.5f);
            yield return new WaitForSeconds(3.5f);
            UIManager.Instance.DisplaySpeech("you: pay jerome when you can. but do it soon, i'm on the line here.", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_OverShoulder);
            UIManager.Instance.DisplaySpeech("bouncer: that i can do. i'll get it to him, i mean it.", 3.5f);
            yield return new WaitForSeconds(3.5f);
            UIManager.Instance.DisplaySpeech("you: i'm probably making a huge mistake. oh well.", 3.5f);
        }

        CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Gym);
        PlayerController.Instance.ToIdle();
        UIManager.Instance.DisplaySpeech("You finished the game! Press esc to quit or r to restart.", 20f);
    }
}
