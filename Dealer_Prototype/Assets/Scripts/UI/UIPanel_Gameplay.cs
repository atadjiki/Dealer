using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPanel_Gameplay : UIPanel
{
    public TextMeshProUGUI Text_GamePlayState;
    public TextMeshProUGUI Text_Money;
    public TextMeshProUGUI Text_Drugs;

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
