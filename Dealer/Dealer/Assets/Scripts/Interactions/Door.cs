using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Door : Interactable
{

    private Transform ExitLocation;
    public CameraManager.SceneCamera OutsideCamera;

    private void Start()
    {
        ExitLocation = GetComponentInChildren<DoorExitLocation>().transform;
    }

    internal override IEnumerator DoInteract()
    {
        yield return base.DoInteract();

        PlayerController.Instance._AI.Teleport(AstarPath.active.GetNearest(ExitLocation.position, NNConstraint.Default).position, true);
        CameraManager.Instance.SetCamera(OutsideCamera);

    }
}
