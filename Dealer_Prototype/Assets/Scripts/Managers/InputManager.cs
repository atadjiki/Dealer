using UnityEngine;

public class InputManager : MonoBehaviour
{
    //singleton stuff 
    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }

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
    }

    private void FixedUpdate()
    {
        if (GameStateManager.Instance.GetMode() == GameStateManager.Mode.GamePlay)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            CameraFollowTarget.Instance.MoveInDirection(new Vector2(x, y));
        }
        else if (GameStateManager.Instance.GetMode() == GameStateManager.Mode.Conversation)
        {

        }
    }
}
