using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public Constants.LevelDataConstants.LevelName Level;

    private void Awake()
    {
        LevelManager.Instance.LoadLevel(Constants.LevelData.GetLevelData(Level));
    }
}
