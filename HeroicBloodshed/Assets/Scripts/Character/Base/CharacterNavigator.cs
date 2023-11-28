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

        _AI.SetPath(path);

        _AI.canMove = true;

        yield return new WaitUntil(() => _AI.reachedEndOfPath);

        _AI.canMove = false;

        _AI.Teleport(destination);
    }
}
