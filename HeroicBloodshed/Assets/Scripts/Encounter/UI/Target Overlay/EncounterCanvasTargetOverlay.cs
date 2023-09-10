using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterCanvasTargetOverlay : EncounterCanvasItemContainer
{
    public override void Populate(EncounterModel model)
    {
        //if(model.GetTargetCandidates().Count > 0)
        //{
        //    CharacterComponent target = model.GetTargetCandidates()[0];
        //    GameObject decal = Instantiate(Prefab_Item, Container);

        //    Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

        //    decal.GetComponent<RectTransform>().position = screenPos;
        //}
    }
}
