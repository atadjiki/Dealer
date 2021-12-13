using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;

    public static CameraController Instance { get { return _instance; } }

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
        Vector3 dirVec = CameraManager.Instance.GetVectorByDirection(direction);

        Vector3 newLocation = this.transform.position + (dirVec) * MoveDistance;

        if(NavigationUtilities.Instance.ValidateDestination(newLocation))
        {
            this.transform.position = NavigationUtilities.Instance.GetValidLocation(newLocation);
        }
    }
}
