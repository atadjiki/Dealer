using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPathCharacter : NPCController
{

    public Collider blockPath;
    private bool busy = false;
    private bool conversationTriggered = false;
    private bool complete = false;

    public List<NPCController> Goons;
    public Transform GoonDispersalPoint;

    public override IEnumerator DoConversation()
    {
        busy = true;
        yield return base.DoConversation();

        if(GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == false
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == false)
        {
            UIManager.Instance.DisplaySpeech("thug: what do you want?", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_AtPlayer);
            UIManager.Instance.DisplaySpeech("you: street closed down?", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_East);
            UIManager.Instance.DisplaySpeech("thug: none of your business. go bother someone else.", 3.5f);
            yield return new WaitForSeconds(3.5f);
        }
        else if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == true
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == false)
        {
            UIManager.Instance.DisplaySpeech("thug: what do you want?", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_AtPlayer);
            UIManager.Instance.DisplaySpeech("you: big jerome sent me here.", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_East);
            UIManager.Instance.DisplaySpeech("thug: you work for that moron? who cares.", 3.5f);
            yield return new WaitForSeconds(3.5f);

            UIManager.Instance.DisplaySpeech("thug: beat it.", 3.5f);
        }
        else if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == true
            && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == true)
        {
            UIManager.Instance.DisplaySpeech("thug: what do you want?", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_AtPlayer);
            UIManager.Instance.DisplaySpeech("you: i'm late for work. at the...'gym'", 3.5f);
            yield return new WaitForSeconds(3.5f);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_East);
            UIManager.Instance.DisplaySpeech("thug: i don't remember you...", 3.5f);
            yield return new WaitForSeconds(3.5f);

            UIManager.Instance.DisplaySpeech("thug: but you do have a badge. alright, go ahead.", 3.5f);
            yield return new WaitForSeconds(3.5f);

            
            UIManager.Instance.DisplaySpeech("thug: don't let me regret this, got it?", 3.5f);
            complete = true;
            Disperse();

        }
        else
        {
            UIManager.Instance.DisplaySpeech("thug: get out of my face.", 3.5f);
        }

        CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_East);
        PlayerController.Instance.ToIdle();
        busy = false;
    }

    private void Update()
    {
        if(CanCharacterUpdate())
        {
            if (complete || PlayerController.Instance == null) { return; }

            if (conversationTriggered == false && busy == false && Vector3.Distance(this.transform.position, PlayerController.Instance.transform.position) < 10)
            {
                conversationTriggered = true;

                PlayerController.Instance.ToTalking();
                UIManager.Instance.SetActionText("");
                StartConversation();
            }
            else if (conversationTriggered && Vector3.Distance(this.transform.position, PlayerController.Instance.transform.position) > 20)
            {
                conversationTriggered = false;
            }
        }
    }

    private void Disperse()
    {
        blockPath.enabled = false;
        AstarPath.active.Scan(AstarPath.active.graphs[0]);

        foreach (NPCController goon in Goons)
        {
            goon.Unavailable = true;
            StartCoroutine(goon.DoMoveToLocation(AstarPath.active.GetNearest(GoonDispersalPoint.position).position + new Vector3(Random.Range(-3,3), 0, Random.Range(-3,3))));

        }
    }
}
