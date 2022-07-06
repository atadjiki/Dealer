using UnityEngine;
using Constants;

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

    private void OnLoadStart(Constants.GameConstants.GameMode levelName)
    {
        allowInput = false;
    }

    private void OnLoadEnd(Constants.GameConstants.GameMode levelName)
    {
        allowInput = true;
    }

    public override bool PerformUpdate(float tick)
    {
        if(base.PerformUpdate(tick))
        {
            if (allowInput)
            {
                if (GameStateManager.Instance.GetGameMode() == State.GameMode.GamePlay)
                {
                    float x = Input.GetAxis("Horizontal");
                    float y = Input.GetAxis("Vertical");

                    CameraFollowTarget.Instance.MoveInDirection(new Vector2(x, y));
                }
                else if (GameStateManager.Instance.GetGamePlayMode() == State.GamePlayMode.Conversation)
                {

                }

                return true;
            }
        }

        return false;
    }
}
