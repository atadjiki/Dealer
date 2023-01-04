using System.Collections;
using Constants;
using GameDelegates;
using UnityEngine;

public class PlayerComponent : NPCComponent, IGameplayInitializer
{
    bool _initialized = false;

    public override void ProcessSpawnData(object _data)
    {
        PlayerSpawnData playerData = (PlayerSpawnData)_data;

        _modelID = playerData.ModelID;

        _team = Enumerations.Team.Player;
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        Instantiate<GameObject>(PrefabLibrary.GetPlayerCanvas(), this.transform);

        OnUpdateCanvas.Invoke("Player");
        OnToggleCanvas.Invoke(true);

        Global.OnNewCameraTarget.Invoke(model.transform);

        model.tag = "Player";

        _initialized = true;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.DrawLine(model.transform.position, hit.point, Color.green, 1.0f);

                    //process the object we hit
                    CharacterModel character = hit.collider.GetComponent<CharacterModel>();
                    PlayerStation station = hit.collider.GetComponent<PlayerStation>();

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
                    else if (station != null)
                    {
                        Debug.Log("Visiting station");
                        GoTo(station.GetEntryTransform().position);
                    }
                    else if (hit.collider.tag == "Ground")
                    {
                        if (NavigationHelper.IsPointValid( hit.point))
                        {
                            GoTo(hit.point);
                        }
                        else
                        {
                            Debug.Log("Path is not possible!");
                        }
                    }
                }
            }
        }
    }

    private void ProcessPendingAction()
    {
        if (Camera.main != null)
        {
            Enumerations.CommandType command = Enumerations.CommandType.None;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //process the object we hit
                CharacterComponent character = hit.collider.GetComponent<CharacterComponent>();
                PlayerStation station = hit.collider.GetComponent<PlayerStation>();

                if (character != null && character.GetType() != typeof(PlayerComponent))
                {
                    float distance = Vector3.Distance(model.transform.position, hit.point);

                    if (distance < 2.5f)
                    {
                        command = Enumerations.CommandType.Interact;
                    }
                    else
                    {
                        command = Enumerations.CommandType.Approach;
                    }
                }
                else if(station != null)
                {
                    command = Enumerations.CommandType.Save;
                }
                else if (hit.collider.tag == "Ground")
                {
                    if (NavigationHelper.IsPointValid(hit.point))
                    {
                        command = Enumerations.CommandType.Move;
                    }
                }
            }

            Global.OnPendingCommandChanged.Invoke(command);
        }
    }

    public bool HasInitialized()
    {
        return _initialized;
    }
}
