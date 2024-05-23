using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Constants;

public class EnvironmentPathDisplay : EncounterEventHandler
{
    [SerializeField] private Material LineMaterial;

    private LineRenderer _lineRenderer;

    private Vector3 _origin;
    private List<Vector3> _range;

    protected override void OnAwake()
    {
        base.OnAwake();

        this.gameObject.layer = LAYER_DECAL;
    }

    public void Setup(CharacterComponent character)
    {
        //set the origin
        _origin = character.GetWorldLocation();

        //figure out what the range is for this character
        _range = EnvironmentUtil.GetCharacterRange(character);

        //setup the line renderer
        _lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = LineMaterial;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    protected override void OnStateChangedCallback(EncounterState state)
    {
        if (state == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        EnvironmentTileRaycastInfo info;
        if(EnvironmentUtil.GetTileBeneathMouse(out info))
        {
            if(info.layer == EnvironmentLayer.GROUND && _range.Contains(info.position))
            {
                List<Vector3> nodes = EnvironmentUtil.CalculatePath(_origin, info.position);

                _lineRenderer.positionCount = nodes.Count;
                _lineRenderer.SetPositions(nodes.ToArray());
                return;
            }
        }

        _lineRenderer.positionCount = 0;
    }
}
