using System.Collections;
using Cinemachine;
using UnityEngine;

public class CutsceneHelper : MonoBehaviour
{
    public static void ProcessCutsceneEvent(CutsceneEvent cutsceneEvent)
    {
        if(cutsceneEvent is TransactionEvent)
        {
            TransactionEvent transactionEvent = (TransactionEvent)cutsceneEvent;

            GameState.HandleTransaction(transactionEvent.Quantity);
        }
        else if(cutsceneEvent is CameraEvent)
        {
            CameraEvent cameraEvent = (CameraEvent)cutsceneEvent;

            CinemachineBrain.SoloCamera = cameraEvent.VirtualCamera;
        }
        else if(cutsceneEvent is AnimationEvent)
        {
            AnimationEvent animationEvent = (AnimationEvent)cutsceneEvent;

            foreach(AnimEventPair pair in animationEvent.Data)
            {
                pair.Model.PlayAnim(pair.Anim);
            }
            
        }
    }
}
