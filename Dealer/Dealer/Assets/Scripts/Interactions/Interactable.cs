using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    private Transform InteractionLocationTransform;

    private void Awake()
    {
        InteractionLocationTransform = GetComponentInChildren<InteractionLocation>().transform;
    }

    public Vector3 GetInteractionLocation()
    {
        return AstarPath.active.GetNearest(InteractionLocationTransform.position, NNConstraint.Default).position;
        
    }

    public virtual void Interact()
    {
        StartCoroutine(DoInteract());
    }

    internal virtual IEnumerator DoInteract()
    {
        yield return null;
    }

    public virtual string GetVerb()
    {
        return "interact";
    }
}
