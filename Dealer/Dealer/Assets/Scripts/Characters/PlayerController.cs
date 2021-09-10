using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class PlayerController : Character
{
    private GameObject SelectionPrefab;
    private HashSet<GameObject> Spawned;
    private float moveRadius = 30;

    private static PlayerController _instance;

    public static PlayerController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        Initialize();

        SelectionPrefab = Resources.Load<GameObject>("SelectionPrefab");
        Spawned = new HashSet<GameObject>();

        DontDestroyOnLoad(this.gameObject);
    }

    public void ReleaseFromChair()
    {
        ToIdle();
        _AI.Teleport(AstarPath.active.GetNearest(this.transform.position).position, true);
    }

    private void Update()
    {
        if(CanCharacterUpdate())
        {
            //if the player is sitting, they can click to stop sitting 
            if (CurrentState == State.Sitting && Input.GetMouseButtonDown(0))
            {
                ReleaseFromChair();
                return;
            }
            else if (CurrentState == State.Sitting)
            {
                UIManager.Instance.SetActionText("get up");
                return;
            }
            //if the player is busy interacting or talking, dont allow input
            else if (CurrentState == Character.State.Talking || CurrentState == Character.State.Interacting)
            {
                UIManager.Instance.SetActionText("");
                return;
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                //if the mouse just hit the ground, move to the specified location
                if (hit.collider.tag == "Ground" && Vector3.Distance(this.transform.position, hit.point) < moveRadius)
                {
                    HandleGroundClick(hit);
                }
                else if (hit.collider.GetComponent<NPCController>() && Vector3.Distance(this.transform.position, hit.point) < moveRadius)
                {
                    HandleNPCClick(hit);
                }
                else if (hit.collider.GetComponent<Interactable>() != null && Vector3.Distance(this.transform.position, hit.point) < moveRadius)
                {
                    HandleInteractableClick(hit);
                }
                else
                {
                    UIManager.Instance.SetActionText("");
                }
            }
            else
            {
                UIManager.Instance.SetActionText("");
            }
        }
        
    }

    private void HandleGroundClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (MoveToLocation(hit.point))
            {
                ClearWaypoints();
                SpawnSelectionPrefab(hit.point);
                StartCoroutine(DoWaitForMovementStopped());
                UIManager.Instance.SetActionText("");
            }
        }
        else if (CurrentState == Character.State.Moving)
        {
            UIManager.Instance.SetActionText("moving");
        }
        else
        {
            UIManager.Instance.SetActionText("move");
        }
    }

    private void HandleNPCClick(RaycastHit hit)
    {
        NPCController hitNPC = hit.collider.GetComponent<NPCController>();

        if (hitNPC.Unavailable)
        {
            UIManager.Instance.SetActionText("");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //if the player isnt close enough, move towards the interaction zone
            if (MoveToLocation(hitNPC.GetConversationLocation()))
            {
                if (Vector3.Distance(this.transform.position, hitNPC.GetConversationLocation()) >= 1f)
                {
                    ClearWaypoints();
                    SpawnSelectionPrefab(hitNPC.GetConversationLocation());
                    StartCoroutine(DoWaitForMovementStopped());
                    UIManager.Instance.SetActionText("");
                }
                else
                {
                    // Debug.Log("talking with " + hitNPC.name);
                    ToTalking();
                    UIManager.Instance.SetActionText("");
                    hitNPC.StartConversation();
                }
            }
        }
        else if (CurrentState == Character.State.Moving)
        {
            UIManager.Instance.SetActionText("moving");
        }
        else if (Vector3.Distance(this.transform.position, hitNPC.GetConversationLocation()) < 1f)
        {
            UIManager.Instance.SetActionText("talk");
        }
        else
        {
            UIManager.Instance.SetActionText("approach");
        }
    }

    private void HandleInteractableClick(RaycastHit hit)
    {
        Interactable hitInteractable = hit.collider.GetComponent<Interactable>();

        if (Input.GetMouseButtonDown(0))
        {
            //if the player isnt close enough, move towards the interaction zone
            if (MoveToLocation(hitInteractable.GetInteractionLocation()))
            {
                if (Vector3.Distance(this.transform.position, hitInteractable.GetInteractionLocation()) >= 1.5f)
                {
                    ClearWaypoints();
                    SpawnSelectionPrefab(hitInteractable.GetInteractionLocation());
                    StartCoroutine(DoWaitForMovementStopped());
                    UIManager.Instance.SetActionText("");
                }
                else
                {
                    UIManager.Instance.SetActionText("");
                    hitInteractable.Interact();
                }
            }
        }
        else if (CurrentState == Character.State.Moving)
        {
            UIManager.Instance.SetActionText("moving");
        }
        else if (Vector3.Distance(this.transform.position, hitInteractable.GetInteractionLocation()) < 1.5f)
        {
            UIManager.Instance.SetActionText(hitInteractable.GetVerb());
        }
        else
        {
            UIManager.Instance.SetActionText("approach");
        }
    }

    private void SpawnSelectionPrefab(UnityEngine.Vector3 location)
    {
        GameObject SelectionEffect = Instantiate<GameObject>(SelectionPrefab, location, SelectionPrefab.transform.rotation, null);
        Spawned.Add(SelectionEffect);
    }

    public void LockControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockControls()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator DoWaitForMovementStopped()
    {
        while (CurrentState == Character.State.Moving)
        {
            yield return null;
        }

        ClearWaypoints();
    }

    public void ClearWaypoints()
    {
        foreach (GameObject obj in Spawned)
        {
            GameObject.Destroy(obj);
        }

        Spawned.Clear();
    }
}
