using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    public Constants.LevelConstants.LevelName LevelName;
    private LevelContent content = null;

    private void Awake()
    {
        content = GetComponentInChildren<LevelContent>();
        content.gameObject.SetActive(false);
        if (SingletonManager.Instance.AreManagersBuilt())
        {
            content.gameObject.SetActive(true);
        }
    }
}
