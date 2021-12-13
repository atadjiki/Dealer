using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;

    public static CameraController Instance { get { return _instance; } }

    public enum CameraTargetState { Free, Attached };

    private CameraTargetState targetState = CameraTargetState.Free;

    public CameraTargetState GetState() { return targetState; }

    [SerializeField] private float MoveDistance = 0.1f;

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

    }

    public void MoveInDirection(InputManager.InputDirection direction)
    {
        if(targetState == CameraTargetState.Free)
        {
            Vector3 dirVec = CameraManager.Instance.GetVectorByDirection(direction);

            Vector3 newLocation = this.transform.position + (dirVec) * MoveDistance;

            if (NavigationUtilities.Instance.ValidateDestination(newLocation))
            {
                this.transform.position = NavigationUtilities.Instance.GetValidLocation(newLocation);
            }
        }
    }

    public void AttachTo(Transform transform)
    {
        this.transform.parent = transform;
        this.transform.position = transform.position;
        targetState = CameraTargetState.Attached;
    }

    public void Release()
    {
        this.transform.parent = CameraManager.Instance.transform;
        this.transform.rotation = Quaternion.identity;
        targetState = CameraTargetState.Free;
    }
}
