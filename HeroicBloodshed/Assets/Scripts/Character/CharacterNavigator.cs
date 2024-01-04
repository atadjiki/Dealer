using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigator : MonoBehaviour
{
    private AILerp _AI;

    private void Awake()
    {
        _AI = GetComponent<AILerp>();
        _AI.canMove = false;
    }

    public IEnumerator Coroutine_MoveToPosition(Vector3 destination)
    {
        ABPath path = ABPath.Construct(this.transform.position, destination);

        Debug.Log("Moving on path to - " + destination.ToString() + " - " + path.GetTotalLength() + " units");

        _AI.SetPath(path);

        _AI.canMove = true;

        yield return new WaitUntil(() => _AI.reachedDestination);

        _AI.canMove = false;

        TeleportTo(destination);
    }

    public void TeleportTo(Vector3 destination)
    {
        Debug.Log("Teleporting to " + destination);
        _AI.Teleport(destination);
    }

    public void Rotate(Quaternion rotation)
    {
        _AI.rotation = rotation;
    }
}
