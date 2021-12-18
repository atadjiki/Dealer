using System.Collections;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    private static CameraFollowTarget _instance;

    public static CameraFollowTarget Instance { get { return _instance; } }

    private enum CameraMovementState { Idle, Moving };

    private CameraMovementState State = CameraMovementState.Idle;

    [SerializeField] private float MoveDistance = 0.5f;
    [SerializeField] private float LerpSpeed = 1.5f;

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

    public void MoveInDirection(Vector2 InputVector)
    {
        Vector3 forward = OverheadCameraManager.Instance.transform.forward;
        var right = OverheadCameraManager.Instance.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * InputVector.y + right * InputVector.x;

        Vector3 newLocation = this.transform.position + desiredMoveDirection * MoveDistance;

        if (NavigationUtilities.Instance.ValidateDestination(newLocation))
        {
            MoveTo(newLocation);
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (State == CameraMovementState.Idle)
        {
            bool valid;
            destination = NavigationUtilities.Instance.GetValidLocation(destination, out valid);

            if(valid)
            {
                StartCoroutine(PerformMove(destination));
            }
        }
    }

    private IEnumerator PerformMove(Vector3 destination)
    {
        State = CameraMovementState.Moving;

        Vector3 origin = this.transform.position;
        
        float fraction = 0;

        while (!Mathf.Approximately(Vector3.Distance(this.transform.position, destination), 0))
        {
            fraction += Time.fixedDeltaTime * LerpSpeed;
            this.transform.position = Vector3.Lerp(origin, destination, fraction);
        }

        State = CameraMovementState.Idle;

        yield return null;
    }
}
