using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterManager : MonoBehaviour
{
    private EncounterPrefabData _prefabData;

    private void Awake()
    {
        _prefabData = ResourceUtil.GetEncounterPrefabs();

        Instantiate<GameObject>(_prefabData.CameraRig);
    }
}
