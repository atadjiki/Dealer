using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

namespace GameDelegates
{
    //character
    public delegate void ShowDecal(Enumerations.Team team);
    public delegate void HideDecal();

    public delegate void ToggleCharacterCanvas(bool flag);
    public delegate void UpdateCharacterCanvas(string name);

    public delegate void NewCameraFollowTarget(Transform transform);

    public delegate void CharacterSpawned(CharacterComponent character);
    public delegate void PendingActionChanged(Enumerations.CharacterAction action);

    public delegate void ModelClicked();

    //npc
    public delegate void NewDestination(Vector3 destination);
    public delegate void NewAction(Enumerations.CharacterAction action);

    public delegate void DestinationReached();

    public class Global
    {
        public static NewCameraFollowTarget OnNewCameraTarget;

        public static CharacterSpawned OnCharacterSpawned;

        public static PendingActionChanged OnPendingActionChanged;

    }
}
