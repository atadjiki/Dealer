using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevelManager : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Build());
    }

    private IEnumerator Build()
    {
     //   yield return new WaitForSeconds(2.0f);

        PrefabFactory.CreatePrefab(Constants.RegistryID.Static_Managers, null);
        PrefabFactory.CreatePrefab(Constants.RegistryID.PerLevel_Managers, null);

        yield return null;
        
    }
}
