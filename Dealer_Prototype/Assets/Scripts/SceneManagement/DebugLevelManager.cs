using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevelManager : MonoBehaviour
{
    private void Awake()
    {
        if (GameStateManager.Instance)
        {
            GameStateManager.Instance.ToMode(GameStateManager.Mode.GamePlay);
        }
    }
}
