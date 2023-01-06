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

    public delegate void MouseContextChanged(Enumerations.MouseContext mouseContext);
    public delegate void MovementStateChanged(Enumerations.MovementState state);

    public delegate void ModelClicked();

    //npc
    public delegate void NewDestination(Vector3 destination);
    public delegate void NewCommand(Enumerations.CommandType command);

    public delegate void PlayerSpawned(PlayerComponent playerComponent);

    public delegate void DestinationReached();

    //ui

    public delegate void SafehouseStationSelected(Enumerations.SafehouseStation station);

    public delegate void BackButtonPressed();
    public delegate void CancelButtonPressed();
    public delegate void OKButtonPressed();

    public delegate void PlayAudioClip(AudioClip clip);

    public class Global
    {
        public static NewCameraFollowTarget OnNewCameraTarget;

        public static MouseContextChanged OnMouseContextChanged;

        public static PlayerSpawned OnPlayerSpawned;

        public static SafehouseStationSelected OnStationSelected;
    }
}
