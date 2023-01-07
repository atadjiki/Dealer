using System.Collections;
using Constants;
using GameDelegates;
using UnityEngine;

public class PlayerComponent : NPCComponent, IGameplayInitializer
{
    bool _initialized = false;

    public override void ProcessSpawnData(CharacterSpawnData _data)
    {
        base.ProcessSpawnData(_data);

        spawnData.ModelID = Enumerations.CharacterModelID.Model_Male_Player;
        spawnData.Team = Enumerations.Team.Player;
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        Instantiate<GameObject>(PrefabLibrary.GetPlayerCanvas(), this.transform);

        if (spawnData.ShowCanvas)
        {
            OnUpdateCanvas.Invoke("Player");
            OnToggleCanvas.Invoke(true);
        }

        if(Global.OnNewCameraTarget != null)
        {
            Global.OnNewCameraTarget.Invoke(model.transform);
        }

        model.tag = "Player";

        _initialized = true;
    }

    void Update()
    {
        if (_initialized == false) return;

     //   ProcessPendingAction();
      //  ProcessMouseEvents();
     //   ProcessKeyEvents();
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
            Enumerations.MouseContext mouseContext = Enumerations.MouseContext.None;

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
                        mouseContext = Enumerations.MouseContext.Interact;
                    }
                    else
                    {
                        mouseContext = Enumerations.MouseContext.Move;
                    }
                }
                else if (hit.collider.tag == "Ground")
                {
                    if (NavigationHelper.IsPointValid(hit.point))
                    {
                        mouseContext = Enumerations.MouseContext.Move;
                    }
                }
            }

            Global.OnMouseContextChanged.Invoke(mouseContext);
        }
    }

    public bool HasInitialized()
    {
        return _initialized;
    }
}
