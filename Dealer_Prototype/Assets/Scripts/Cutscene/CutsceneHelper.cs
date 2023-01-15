using System.Collections;
using UnityEngine;

public class CutsceneHelper : MonoBehaviour
{
    public static IEnumerator ProcessCutsceneEvent(CutsceneEvent cutsceneEvent)
    {
        if(cutsceneEvent is TransactionEvent)
        {
            TransactionEvent transactionEvent = (TransactionEvent)cutsceneEvent;

            GameState.HandleTransaction(transactionEvent.Quantity);
        }

        yield return null; ;
    }
}
