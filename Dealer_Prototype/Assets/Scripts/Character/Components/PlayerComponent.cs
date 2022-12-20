using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerComponent : NPCComponent
{
    public delegate void OnPlayerSpawned(Transform transform);
    public static OnPlayerSpawned OnPlayerSpawnedDelegate;

    public override void ProcessSpawnData(object _data)
    {
        PlayerSpawnData playerData = (PlayerSpawnData)_data;

        _modelID = playerData.ModelID;
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        OnPlayerSpawnedDelegate.Invoke(model.transform);

        characterCanvas.Toggle(true);
        characterCanvas.SetName(_modelID.ToString());
    }

    protected override void ShowGroundDecal()
    {
        MaterialHelper.SetPlayerGroundDecal(groundDecal);
    }

    protected override void Highlight()
    {
        MaterialHelper.SetPlayerOutline(model);
    }

    void Update()
    {
        if(Camera.main != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.DrawLine(model.transform.position, hit.point, Color.green, 1.0f);

                    //process the object we hit
                    if (hit.collider.GetComponent<CharacterComponent>() != null)
                    {
                        Debug.Log("hit character");

                        float distance = Vector3.Distance(model.transform.position, hit.point);

                        if(distance < 1.5f)
                        {
                            Debug.Log("interact with character");
                        }
                        else
                        {
                            GoTo(hit.point);
                        }
                    }
                    else if(hit.collider.tag == "Ground")
                    {
                        Debug.Log("hit ground");
                        GoTo(hit.point);
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Stop();
        }
    }
}
