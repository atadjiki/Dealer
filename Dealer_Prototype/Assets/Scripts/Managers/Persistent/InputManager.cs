using UnityEngine;

public class InputManager : Manager
{
    private bool allowInput = false;

    //singleton stuff 
    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }

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

        base.Build();
    }

    public override int AssignDelegates()
    {
        LevelManager.Instance.onLoadStart += OnLoadStart;
        LevelManager.Instance.onLoadEnd += OnLoadEnd;

        return 2;
    }

    private void OnLoadStart(Constants.LevelConstants.LevelName levelName)
    {
        allowInput = false;
    }

    private void OnLoadEnd(Constants.LevelConstants.LevelName levelName)
    {
        allowInput = true;
    }

    public override bool PerformUpdate(float tick)
    {
        if(base.PerformUpdate(tick))
        {
            if (allowInput)
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

                return true;
            }
        }

        return false;
    }
}
