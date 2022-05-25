using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class OverheadCameraManager : MonoBehaviour
{
    private static OverheadCameraManager _instance;

    public static OverheadCameraManager Instance { get { return _instance; } }

    private CinemachineVirtualCamera CM_Main;

    private int characterLayerMask;
    private int doorLayerMask;
    private int doorFrameLayerMask;
    private int groundLayerMask;
    private int interactableLayerMask;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        CM_Main = GetComponent<CinemachineVirtualCamera>();

        characterLayerMask = 1 << LayerMask.NameToLayer("Character");
        doorLayerMask = 1 << LayerMask.NameToLayer("Door");
        doorFrameLayerMask = 1 << LayerMask.NameToLayer("DoorFrame");
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        interactableLayerMask = 1 << LayerMask.NameToLayer("Interactable");
    }

    public void SetCullingMask()
    {
        Camera.main.cullingMask = characterLayerMask | doorFrameLayerMask | doorLayerMask
            | groundLayerMask | interactableLayerMask;
    }
}
