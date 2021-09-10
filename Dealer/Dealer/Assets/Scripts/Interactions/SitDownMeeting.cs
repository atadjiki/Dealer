using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SitDownMeeting : Chair
{

    public CinemachineVirtualCamera CM_Boss;
    public CinemachineVirtualCamera CM_Henchman;
    public CinemachineVirtualCamera CM_AtSitting;
    public CinemachineVirtualCamera CM_Meeting;

    private float Time_Dialogue = 3.5f;
    private float Time_Camera = 3.5f;

    private void Start()
    {
        CameraManager.Instance.RegisterCamera(CM_Boss);
        CameraManager.Instance.RegisterCamera(CM_Henchman);
        CameraManager.Instance.RegisterCamera(CM_AtSitting);
        CameraManager.Instance.RegisterCamera(CM_Meeting);
    }

    internal override IEnumerator DoInteract()
    {

        if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == true
           && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == false)
        {
            UIManager.Instance.DisplaySpeech("henchman: the kid you need to talk to is out back.", Time_Dialogue);
            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_North);
            PlayerController.Instance.ToIdle();
        }
        else if (GameState.Instance.IsObjectiveCompleted(GameState.Objectives.CompletedSitDown) == true
           && GameState.Instance.IsObjectiveCompleted(GameState.Objectives.ReceivedBadge) == true)
        {
            UIManager.Instance.DisplaySpeech("henchman: you got the badge? you can find the gym down the street.", Time_Dialogue);
            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_North);
            PlayerController.Instance.ToIdle();
        }
        else
        {
            yield return StartCoroutine(base.DoInteract());

            PlayerController.Instance.CurrentState = Character.State.Talking;
            CameraManager.Instance.SwitchToCamera(CM_Meeting);
            UIManager.Instance.DisplaySpeech("mysterious man: finally, there he is. take a seat.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_Boss);
            UIManager.Instance.DisplaySpeech("mysterious man: i don't believe you gentlemen have met.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera / 2);

            CameraManager.Instance.SwitchToCamera(CM_Meeting);
            yield return new WaitForSeconds(Time_Camera / 2);
            UIManager.Instance.DisplaySpeech("mysterious man: this is my assistant, HENCHMAN, maybe you've heard of him.", Time_Dialogue);
            CameraManager.Instance.SwitchToCamera(CM_AtSitting);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_Henchman);
            UIManager.Instance.DisplaySpeech("henchman: cheers mate.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_Boss);
            UIManager.Instance.DisplaySpeech("mysterious man: and of course, i'm BIG JEROME.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);
            UIManager.Instance.DisplaySpeech("big jerome: you could say i run things around here.", Time_Dialogue);
            yield return new WaitForSeconds(4.5f);

            CameraManager.Instance.SwitchToCamera(CM_Boss);
            UIManager.Instance.DisplaySpeech("big jerome: now, your task.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Dialogue);
            UIManager.Instance.DisplaySpeech("big jerome: i need you to find out who's been robbing me blind.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Dialogue);

            CameraManager.Instance.SwitchToCamera(CM_AtSitting);
            UIManager.Instance.DisplaySpeech("you: sounds easy enough.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_Henchman);
            UIManager.Instance.DisplaySpeech("henchman: you having a laugh?", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_Boss);
            UIManager.Instance.DisplaySpeech("big jerome: shut up the both of you.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_Meeting);
            UIManager.Instance.DisplaySpeech("big jerome: i've been told it's someone with a gambling problem, as if that narrows it down.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);
            UIManager.Instance.DisplaySpeech("big jerome: you want to go to the boxing gym down the alley. word is they've turned it into a casino.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);
            UIManager.Instance.DisplaySpeech("big jerome: bring me an answer and i'll make it worth your while.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);
            UIManager.Instance.DisplaySpeech("big jerome: you'll be making more than anyone in this recession.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_AtSitting);
            UIManager.Instance.DisplaySpeech("you: hey, i'll do anything to get rid of my student loans.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_Boss);
            UIManager.Instance.DisplaySpeech("big jerome: good. talk to that kid out back. he might be able to help.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SwitchToCamera(CM_Meeting);
            UIManager.Instance.DisplaySpeech("big jerome: get the hell out of here. don't mess this up.", Time_Dialogue);
            yield return new WaitForSeconds(Time_Camera);

            CameraManager.Instance.SetCamera(CameraManager.SceneCamera.CM_Player_North);
            PlayerController.Instance.ReleaseFromChair();
            GameState.Instance.ObjectiveCompleted(GameState.Objectives.CompletedSitDown);
        }
        
    }

    public override string GetVerb()
    {
        return "begin meeting";
    }
}
