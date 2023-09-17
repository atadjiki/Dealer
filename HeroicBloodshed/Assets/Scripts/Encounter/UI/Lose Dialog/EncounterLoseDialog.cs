using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterLoseDialog : EncounterUIElement
{
    [Header("Variables")]
    [SerializeField] private Button Button_TryAgain;
    [SerializeField] private Button Button_Exit;

    private void Awake()
    {
        //assign delegates
        Button_TryAgain.onClick.AddListener(() => OnTryAgainButtonPressed());
        Button_Exit.onClick.AddListener(() => OnExitButtonPressed());
    }

    public override void Populate(EncounterModel model)
    {
        base.Populate(model);
    }

    public override void HandleStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.DONE:
                {
                    if(!model.DidPlayerWin())
                    {
                        Populate(model);
                        Show();
                    }
                    else
                    {
                        Hide();
                    }
                    break;
                }
            default:
                {
                    Hide();
                    break;
                }
        }
    }

    private void OnTryAgainButtonPressed()
    {
        Debug.Log("Try again!");
    }

    private void OnExitButtonPressed()
    {
        //tell encounter manager to clean up 
        GameObject.Destroy(this.gameObject);
    }
}
