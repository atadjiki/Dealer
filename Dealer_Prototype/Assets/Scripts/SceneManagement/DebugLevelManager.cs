using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevelManager : MonoBehaviour
{
    private void Awake()
    {
        GameState.Instance.ToState(GameState.State.GamePlay);
    }
}
