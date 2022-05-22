using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct CharacterInfo
{
    public CharacterConstants.CharacterID ID;
    public AnimationConstants.Anim InitialAnim;
    public string name;

    public string toString()
    {
        return

            "Character Info:\n" +
            "Name: " + name + "\n" +
            "Model: " + ID.ToString() + "\n" +
            "Initial Anim : " + InitialAnim.ToString() + "\n"

        ;
    }
}

[System.Serializable]
public struct GameState 
{
    [SerializeField] public CharacterInfo playerInfo;
    [SerializeField] public CharacterInfo[] partyInfo;
    [SerializeField] public float money;

    public string toString()
    {
        string result = "Game State:\n";

        result += "Money: " + money + "\n";

        result += playerInfo.toString();

        foreach(CharacterInfo info in partyInfo)
        {
            result += info.toString();
        }

        return result;
    }
}
