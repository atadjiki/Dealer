using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPanelList : MonoBehaviour
{
    public List<PartyListItem> Items;

    const int capacity = 4;
    private CharacterComponent[] characters;

    private static PartyPanelList _instance;

    public static PartyPanelList Instance { get { return _instance; } }

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
        characters = new CharacterComponent[capacity];

        UpdateList();
    }

    private void UpdateList()
    {
        for(int i = 0; i < capacity; i++)
        {
            PartyListItem listItem = Items[i];

            if (characters[i] != null)
            {
                string characterName = characters[i].GetID().ToString();

                string task = "task";

                string days = "0d";

                listItem.SetText(characterName + " - " + task + " - " + days);

                listItem.ToggleVisiblity(true);
            }
            else
            {
                listItem.ToggleVisiblity(false);
            }
        }
    }

    public void RegisterCharacter(CharacterComponent character)
    {
        for(int i = 0; i < capacity; i++)
        {
            if(characters[i] == null)
            {
                characters[i] = character;
                UpdateList();
                break;
            }
        }
    }

    public void UnRegisterCharacter(CharacterComponent character)
    {
        for (int i = 0; i < capacity; i++)
        {
            if (characters[i] == character)
            {
                characters[i] = null;
                UpdateList();
                break;
            }
        }
    }
}
