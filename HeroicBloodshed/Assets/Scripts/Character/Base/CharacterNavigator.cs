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

    public IEnumerator MoveToTile(EnvironmentTile tile)
    {
        Vector3 destination = tile.transform.position;

        ABPath path = ABPath.Construct(this.transform.position, tile.transform.position);
        NNConstraint constraint = new NNConstraint();

        _AI.SetPath(path);

        _AI.canMove = true;
        _AI.destination = destination;

        yield return new WaitWhile(() => Vector3.Distance(destination, this.transform.position) > 0.1f);
    }


}
