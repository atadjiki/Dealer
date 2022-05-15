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
        if (GameState.Instance.GetState() == GameState.State.GamePlay)
        {
        }
        else if (GameState.Instance.GetState() == GameState.State.Conversation)
        {

        }
    }
}
