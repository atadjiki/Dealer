using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct ModelInfo
{
    public ModelID ID;
    public GameObject Model;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterModelData", order = 1)]
public class CharacterModelData : ScriptableObject
{
    [SerializeField] private List<ModelInfo> Models;

    public GameObject GetModel(ModelID ID)
    {
        foreach(ModelInfo info in Models)
        {
            if(info.ID == ID)
            {
                return info.Model;
            }
        }

        return null;
    }
}
