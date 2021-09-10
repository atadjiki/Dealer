using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Chair : Interactable
{
    private Vector3 localSitPosition = new Vector3(0,-1,0.6f);

    internal override IEnumerator DoInteract()
    {
        yield return StartCoroutine(base.DoInteract());

        Transform initialTransform = PlayerController.Instance.transform;
        PlayerController.Instance.transform.parent = this.transform;
        PlayerController.Instance.transform.localPosition = localSitPosition;
        PlayerController.Instance.transform.localEulerAngles = Vector3.zero;

        PlayerController.Instance.ToSitting();

    }

    public override string GetVerb()
    {
        return "sit";
    }
}
