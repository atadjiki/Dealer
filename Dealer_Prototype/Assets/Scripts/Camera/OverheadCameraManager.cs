using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class OverheadCameraManager : MonoBehaviour
{
    private static OverheadCameraManager _instance;

    public static OverheadCameraManager Instance { get { return _instance; } }

    private CinemachineVirtualCamera CM_Main;

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
    }
}
