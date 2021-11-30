using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * This used to be like an NPC Component, but i think it would be better
 * for the player to instead be able to "possess" any character and interact with them
 */
public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;

    private NPCComponent _possesedCharacter;

    public static PlayerController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {

    }
}
