using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSetter : MonoBehaviour
{
    public GameState.State state;

    private void Awake()
    {
        GameState.Instance.ToState(state);
    }
}
