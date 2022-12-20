using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerComponent : NPCComponent, IGameplayInitializer
{
    bool _initialized = false;

    public delegate void OnPlayerSpawned(Transform transform);
    public static OnPlayerSpawned OnPlayerSpawnedDelegate;

    public delegate void OnPlayerCursorContextChanged(PlayerCanvas.CursorContext context);
    public static OnPlayerCursorContextChanged OnPlayerCursorContextChangedDelegate;

    public override void ProcessSpawnData(object _data)
    {
        PlayerSpawnData playerData = (PlayerSpawnData)_data;

        _modelID = playerData.ModelID;
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        OnPlayerSpawnedDelegate.Invoke(model.transform);
        OnPlayerCursorContextChangedDelegate.Invoke(PlayerCanvas.CursorContext.None);

        yield return new WaitForSeconds(0.25f);

        //add player canvas
        Instantiate<GameObject>(PrefabLibrary.GetPlayerCanvas(), this.transform);

        characterCanvas.Toggle(true);
        characterCanvas.SetName(_modelID.ToString());

        _initialized = true;
    }

    protected override void ShowGroundDecal()
    {
        MaterialHelper.SetPlayerGroundDecal(groundDecal);
    }

    public override void Highlight()
    {
        MaterialHelper.SetPlayerOutline(model);
    }

    void Update()
    {
        if (!_initialized) return;

        if(Camera.main != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.DrawLine(model.transform.position, hit.point, Color.green, 1.0f);

                    //process the object we hit
                    CharacterComponent character = hit.collider.GetComponent<CharacterComponent>();

                    if (character != null)
                    {
                        Debug.Log("hit character");

                        float distance = Vector3.Distance(model.transform.position, hit.point);

                        if(distance < 2.5f)
                        {
                            Debug.Log("interact with character");
                            OnPlayerCursorContextChangedDelegate.Invoke(PlayerCanvas.CursorContext.Interact);
                        }
                        else
                        {
                            GoTo(hit.point);
                            OnPlayerCursorContextChangedDelegate.Invoke(PlayerCanvas.CursorContext.Move);
                        }
                    }
                    else if(hit.collider.tag == "Ground")
                    {
                        GoTo(hit.point);
                        OnPlayerCursorContextChangedDelegate.Invoke(PlayerCanvas.CursorContext.Move);
                    }
                }
            }
            else
            {
                OnPlayerCursorContextChangedDelegate.Invoke(PlayerCanvas.CursorContext.None);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Stop();
        }
    }

    public bool HasInitialized()
    {
        return _initialized;
    }
}
