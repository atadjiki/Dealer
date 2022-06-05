using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Constants;

public class Panel_InGame_Day_Party : UIPanel
{
    public GameObject Text_Title;
    public List<Party_InGame_Day_Party_ListItem> Items;

    public override void OnGamePlayModeChanged(State.GamePlayMode GamePlayMode)
    {
        switch(GamePlayMode)
        {
            case State.GamePlayMode.Day:
                ShowPanel();
                allowUpdate = true;
                break;
            default:
                HidePanel();
                allowUpdate = false;
                break;
        }
    }

    public override void OnCharacterManagerUpdate()
    {
        UpdatePanel();
    }

    public override void ShowPanel()
    {
        Text_Title.SetActive(true);
        UpdatePanel();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        Text_Title.SetActive(false);
        base.HidePanel();
    }

    public override void UpdatePanel()
    {
       if(allowUpdate)
        {
            List<CharacterComponent> Party = CharacterManager.Instance.GetParty();

            if (Party != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Party_InGame_Day_Party_ListItem listItem = Items[i];

                    if (i < Party.Count && listItem != null)
                    {
                        CharacterComponent character = Party[i];

                        string name = character.GetCharacterInfo().name;

                        CharacterTask characterTask = Party[i].GetTaskComponent().GetTask();

                        string state = " - " + Constants.CharacterConstants.StateToString(character.GetState());

                        string taskDays;
                        if (characterTask.DaysRemaining > 0)
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
                    else if (listItem == null)
                    {
                        //  Debug.Log("list item is null");
                    }
                    else
                    {
                        listItem.ToggleVisiblity(false);
                    }
                }
            }
        }
    }
}
