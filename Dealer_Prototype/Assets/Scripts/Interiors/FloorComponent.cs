using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class FloorComponent : MonoBehaviour
{
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 location = hit.collider.gameObject.transform.position;
            PlayableCharacterManager.Instance.AttemptMoveOnPossesedCharacter(location);
        }
    }

    private void OnMouseEnter()
    {
        CursorManager.Instance.ToDefault();
        Debug.Log(this.gameObject.name);
    }
}
