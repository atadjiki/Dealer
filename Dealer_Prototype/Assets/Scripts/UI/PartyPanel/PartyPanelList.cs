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
        List<CharacterComponent> Party = CharacterManager.Instance.GetParty();

        if(Party != null)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                PartyListItem listItem = Items[i];

                if (i < Party.Count)
                {
                    CharacterComponent character = Party[i];

                    string name = character.GetCharacterInfo().name;

                    CharacterTask characterTask = Party[i].GetTaskComponent().GetTask();

                    string state = " - " + Constants.CharacterConstants.StateToString(character.GetState()); 

                    string taskDays;
                    if (characterTask.DaysRemaining > 0 )
                    {
                        taskDays = " - " + characterTask.DaysRemaining + "d"; 
                    }
                    else
                    {
                        taskDays = "";
                    }

                    listItem.ToggleVisiblity(true);

                    listItem.SetText(name + state + taskDays);

                    if (Party[i].GetAnimationComponent().IsVisible())
                    {
                        listItem.SetTextColor(Color.yellow);
                    }
                    else
                    {
                        listItem.SetTextColor(Color.gray);
                    }
                }
                else
                {
                    listItem.ToggleVisiblity(false);
                }
            }
        }
    }
}
