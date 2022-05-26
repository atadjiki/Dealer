using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class OverheadCameraManager : Manager
{
    private static OverheadCameraManager _instance;

    public static OverheadCameraManager Instance { get { return _instance; } }

    private int characterLayerMask;
    private int doorLayerMask;
    private int doorFrameLayerMask;
    private int groundLayerMask;
    private int interactableLayerMask;

    public override void Build()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        characterLayerMask = 1 << LayerMask.NameToLayer("Character");
        doorLayerMask = 1 << LayerMask.NameToLayer("Door");
        doorFrameLayerMask = 1 << LayerMask.NameToLayer("DoorFrame");
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        interactableLayerMask = 1 << LayerMask.NameToLayer("Interactable");

        base.Build();
    }

    public void SetCullingMask()
    {
        Camera.main.cullingMask = characterLayerMask | doorFrameLayerMask | doorLayerMask
            | groundLayerMask | interactableLayerMask;
    }
}
