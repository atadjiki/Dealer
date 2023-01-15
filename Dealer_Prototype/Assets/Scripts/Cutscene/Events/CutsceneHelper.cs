using System.Collections;
using System.Collections.Generic;
using GameDelegates;
using UnityEngine;

public class CutsceneHelper 
{
    public static void ProcessCutsceneEvent(CutsceneEvent cutsceneEvent)
    {
        if(cutsceneEvent is TransactionEvent)
        {
            TransactionEvent transactionEvent = (TransactionEvent)cutsceneEvent;

            GameState.HandleTransaction(transactionEvent.Quantity);
        }
    }
}
