using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPanelList : MonoBehaviour
{
    public List<PartyListItem> Items;

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
        UpdateList();
    }

    public void UpdateList()
    {
        List<CharacterInfo> Party = PartyManager.Instance.GetParty();

        for(int i = 0; i < Items.Count; i++)
        {
            PartyListItem listItem = Items[i];

            if (i < Party.Count)
            {
                string characterName = Party[i].ID.ToString();

                string task = "task";

                string days = "0d";

                listItem.ToggleVisiblity(true);

                listItem.SetText(characterName + " - " + task + " - " + days);
            }
            else
            {
                listItem.ToggleVisiblity(false);
            }
        }
    }
}
