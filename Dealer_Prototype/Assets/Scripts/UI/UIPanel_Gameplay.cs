using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Constants;

public class UIPanel_Gameplay : UIPanel, IEventReceiver
{
    public TextMeshProUGUI Text_GamePlayState;
    public TextMeshProUGUI Text_Money;
    public TextMeshProUGUI Text_Drugs;

    private void Start()
    {
        EventManager.Instance.RegisterReceiver(this);

        PerformUpdate();
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnregisterReceiver(this);
    }

    public void HandleEvent(Enumerations.EventID eventID)
    {
        PerformUpdate();
    }

    public override void PerformUpdate()
    {
        base.PerformUpdate();

        GameState gameState = GameStateManager.Instance.GetGameState();

        Text_GamePlayState.text = gameState.GetEnvironment().ToString();

        PlayerData playerData = gameState.GetPlayerData();

        Text_Money.text = "$" + playerData.Money.ToString();
        Text_Drugs.text = playerData.Drugs.ToString();
    }
}
