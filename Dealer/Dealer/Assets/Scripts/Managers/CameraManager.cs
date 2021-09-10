using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance { get { return _instance; } }

    public enum SceneCamera { CM_Player_North, CM_Player_South, CM_Player_East, CM_Gym, CM_OverShoulder, CM_AtPlayer, CM_Side };

    private List<CinemachineVirtualCamera> Cameras;

    public CinemachineVirtualCamera CM_Player_North;
    public CinemachineVirtualCamera CM_Player_South;
    public CinemachineVirtualCamera CM_Player_East;
    public CinemachineVirtualCamera CM_OverShoulder;
    public CinemachineVirtualCamera CM_AtPlayer;
    public CinemachineVirtualCamera CM_Side;
    public CinemachineVirtualCamera CM_Gym;

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
        Cameras = new List<CinemachineVirtualCamera>() { CM_Player_North, CM_Player_South, CM_Player_East, CM_Gym, CM_OverShoulder, CM_AtPlayer, CM_Side };
    }

    private void SetAll(bool flag)
    {
        foreach(CinemachineVirtualCamera cam in Cameras)
        {
            cam.enabled = flag;
        }
    }

    public void SetCamera(SceneCamera cam)
    {
        SetAll(false);

        if(cam == SceneCamera.CM_Player_North)
        {
            CM_Player_North.enabled = true;
        }
        else if (cam == SceneCamera.CM_Player_South)
        {
            CM_Player_South.enabled = true;
        }
        else if( cam == SceneCamera.CM_Player_East)
        {
            CM_Player_East.enabled = true;
        }
        else if(cam == SceneCamera.CM_OverShoulder)
        {
            CM_OverShoulder.enabled = true;
        }
        else if(cam == SceneCamera.CM_AtPlayer)
        {
            CM_AtPlayer.enabled = true;
        }
        else if(cam == SceneCamera.CM_Side)
        {
            CM_Side.enabled = true;
        }
        else if(cam == SceneCamera.CM_Gym)
        {
            CM_Gym.enabled = true;
        }
    }

    public void RegisterCamera(CinemachineVirtualCamera customCam)
    {
        Cameras.Add(customCam);
    }

    public void SwitchToCamera(CinemachineVirtualCamera customCam)
    {
        SetAll(false);
        customCam.enabled = true;
    }
}
