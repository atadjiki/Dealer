using System.Collections;
using Constants;
using UnityEngine;

public class PlayerComponent : NPCComponent, IGameplayInitializer
{
    bool _initialized = false;

    public delegate void OnPlayerSpawned(Transform transform);
    public static OnPlayerSpawned OnPlayerSpawnedDelegate;

    public delegate void OnPendingActionChanged(Enumerations.CharacterAction action);
    public static OnPendingActionChanged OnPendingActionChangedDelegate;

    public override void ProcessSpawnData(object _data)
    {
        PlayerSpawnData playerData = (PlayerSpawnData)_data;

        _modelID = playerData.ModelID;
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        OnPlayerSpawnedDelegate.Invoke(model.transform);

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
        if (_initialized == false) return;

        ProcessPendingAction();
        ProcessMouseEvents();
        ProcessKeyEvents();
    }

    private void ProcessKeyEvents()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Stop();
        }
    }

    private void ProcessMouseEvents()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.DrawLine(model.transform.position, hit.point, Color.green, 1.0f);

                    //process the object we hit
                    CharacterComponent character = hit.collider.GetComponent<CharacterComponent>();

                    if (character != null)
                    {
                        float distance = Vector3.Distance(model.transform.position, hit.point);

                        if (distance < 2.5f)
                        {
                            Debug.Log("TODO - interact");
                        }
                        else
                        {
                            //TODO - characters should have a location in front of them that is approachable
                            GoTo(hit.point);
                        }
                    }
                    else if (hit.collider.tag == "Ground")
                    {
                        GoTo(hit.point);
                    }
                }
            }
        }
    }

    private void ProcessPendingAction()
    {
        if (Camera.main != null)
        {
            Enumerations.CharacterAction pendingAction = Enumerations.CharacterAction.None;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //process the object we hit
                CharacterComponent character = hit.collider.GetComponent<CharacterComponent>();

                if (character != null && character.GetType() != typeof(PlayerComponent))
                {
                    float distance = Vector3.Distance(model.transform.position, hit.point);

                    if (distance < 2.5f)
                    {
                        pendingAction = Enumerations.CharacterAction.Interact;
                    }
                    else
                    {
                        pendingAction = Enumerations.CharacterAction.Approach;
                    }
                }
                else if (hit.collider.tag == "Ground")
                {
                    pendingAction = Enumerations.CharacterAction.Move;
                }
            }

            OnPendingActionChangedDelegate.Invoke(pendingAction);
        }
    }

    public bool HasInitialized()
    {
        return _initialized;
    }
}
