using System.Collections;
using Constants;
using UnityEngine;

public class CutsceneHelper : MonoBehaviour
{
    public static void ProcessCutsceneEvent(Cutscene cutscene, CutsceneEvent cutsceneEvent)
    {
        if(cutsceneEvent is TransactionEvent)
        {
            TransactionEvent transactionEvent = (TransactionEvent)cutsceneEvent;

            GameState.HandleTransaction(transactionEvent.Quantity);
        }
        else if(cutsceneEvent is CameraEvent)
        {
            CameraEvent cameraEvent = (CameraEvent)cutsceneEvent;

            //CinemachineBrain.SoloCamera = cameraEvent.VirtualCamera;
        }
        else if(cutsceneEvent is AnimationEvent)
        {
            AnimationEvent animationEvent = (AnimationEvent)cutsceneEvent;

            foreach(AnimEventPair pair in animationEvent.Data)
            {
                CutsceneCharacterComponent character = cutscene.FindCharacter(pair.CharacterID);
                if(character != null)
                {
                    character.PlayAnim(pair.Anim);
                }
            } 
        }
        else if(cutsceneEvent is CharacterVisiblityEvent)
        {
            CharacterVisiblityEvent visiblityEvent = (CharacterVisiblityEvent)cutsceneEvent;

            CutsceneCharacterComponent characterComponent = cutscene.FindCharacter(visiblityEvent.CharacterID);

            if(characterComponent != null)
            {
                characterComponent.ToggleVisiblity(visiblityEvent.Visibility);
            }
        }
    }

    public static CutsceneCharacterComponent SpawnCutsceneCharacter(Cutscene cutscene, CutsceneCharacterData data)
    {
        if (data.Model == Enumerations.CharacterModelID.None) { return null; }

        GameObject characterObject = new GameObject("Cutscene_ " + data.Model, new System.Type[] { typeof(CutsceneCharacterComponent) });
        characterObject.transform.parent = cutscene.gameObject.transform;
        characterObject.transform.position = data.SpawnLocation.position;
        characterObject.transform.rotation = data.SpawnLocation.rotation;

        CutsceneCharacterComponent cutsceneCharacter = characterObject.GetComponent<CutsceneCharacterComponent>();

        CharacterSpawnData spawnData = new CharacterSpawnData();
        spawnData.ModelID = data.Model;

        cutsceneCharacter.CharacterID = data.ID;
        cutsceneCharacter.ProcessSpawnData(spawnData);
        cutsceneCharacter.Initialize();

        return cutsceneCharacter;
    }
}

