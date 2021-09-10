using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetUrchin : NPCController
{

    public override IEnumerator DoConversation()
    {
        PlayerController.Instance.transform.LookAt(this.transform);

        //if the player has not talked to them yet, done the sit down, or received the badge
        if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == false 
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.TalkedToStreetUrchin) == false
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == false)
        {
            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_OverShoulder);
            UIManager.Instance.DisplaySpeech("street urchin: what are you looking at?", SpeechTime);
            yield return new WaitForSeconds(CameraTime);
            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_AtPlayer);
            UIManager.Instance.DisplaySpeech("you: nothing.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);
            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_South);
            yield return null;

            PlayerController.Instance.ToIdle();
        }
        //if the player has completed the sitdown, but not yet received the badge, and has never talked to the urchin
        else if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == true
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.TalkedToStreetUrchin) == false
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == false)
        {
            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_OverShoulder);
            UIManager.Instance.DisplaySpeech("street urchin: what do you want?", SpeechTime);
            yield return new WaitForSeconds(CameraTime);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_AtPlayer);
            UIManager.Instance.DisplaySpeech("you: jerome told me to talk to you.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_OverShoulder);
            UIManager.Instance.DisplaySpeech("street urchin: big jerome? oh. i remember, here.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);
            UIManager.Instance.DisplaySpeech("street urchin: this badge should get you into the casino, i work there.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);
            UIManager.Instance.DisplaySpeech("street urchin: now beat it, will you? i'm having my smoke break.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_South);
            yield return new WaitForSeconds(CameraTime);
            PlayerController.Instance.ToIdle();
            GameState.Instance.ObjectiveCompleted(GameState.Objectives.ReceivedBadge);

        }
        //if the player talked to the urchin, and is now talking to them again after doing the sit down
        else if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == true
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.TalkedToStreetUrchin) == true
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == false)
        {
            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_OverShoulder);
            UIManager.Instance.DisplaySpeech("street urchin: you again? stop bothering me.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_AtPlayer);
            UIManager.Instance.DisplaySpeech("you: big jerome told me to talk to you.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_OverShoulder);
            UIManager.Instance.DisplaySpeech("street urchin: big jerome? why didn't you say so? jesus.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);
            UIManager.Instance.DisplaySpeech("street urchin: here, take this badge to get in the boxing gym.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);
            UIManager.Instance.DisplaySpeech("street urchin: tell jerome i said hi. or whatever.", SpeechTime);
            yield return new WaitForSeconds(CameraTime);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_South);
            yield return new WaitForSeconds(CameraTime);
            PlayerController.Instance.ToIdle();
            GameState.Instance.ObjectiveCompleted(GameState.Objectives.ReceivedBadge);

        }
        else if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == false
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.TalkedToStreetUrchin) == true
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == false)
        {
            UIManager.Instance.DisplaySpeech("street urchin: didn't i tell you to fuck off?", SpeechTime);
            yield return null;

        }
        else if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == true
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.TalkedToStreetUrchin) == true
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == true)
        {
            UIManager.Instance.DisplaySpeech("street urchin: look, i gave you the badge, just go to the boxing gym.", SpeechTime);
            yield return null;

        }

        GameState.Instance.ObjectiveCompleted(GameState.Objectives.TalkedToStreetUrchin);
        PlayerController.Instance.ToIdle();
        yield return null;
    }
}
