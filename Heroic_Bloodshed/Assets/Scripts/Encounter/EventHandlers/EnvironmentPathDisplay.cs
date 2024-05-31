using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Constants;

public class EnvironmentPathDisplay : EncounterEventHandler
{
    private LineRenderer _lineRenderer;

    private Vector3 _origin;
    private Dictionary<MovementRangeType, List<Vector3>> _rangeMap;

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
        _rangeMap = EnvironmentUtil.GetCharacterRangeMap(character);

        //setup the line renderer
        _lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = MaterialLibrary.Get(MaterialID.PATH_DISPLAY);
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    protected override void OnStateChangedCallback(EncounterStateData stateData)
    {
        if (stateData.GetCurrentState() == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        EnvironmentTileRaycastInfo info;
        if(EnvironmentUtil.GetTileBeneathMouse(out info))
        {
            MovementRangeType rangeType = GetRangeTypeFromPoint(info.position);

            if (info.layer == EnvironmentLayer.GROUND && rangeType != MovementRangeType.NONE)
            {
                List<Vector3> nodes = EnvironmentUtil.CalculatePath(_origin, info.position);

                _lineRenderer.positionCount = nodes.Count;
                _lineRenderer.SetPositions(nodes.ToArray());

                _lineRenderer.material.color = ColorLibrary.Get(rangeType);

                return;
            }
        }

        _lineRenderer.positionCount = 0;
    }

    private MovementRangeType GetRangeTypeFromPoint(Vector3 point)
    {
        if (_rangeMap[MovementRangeType.HALF].Contains(point))
        {
            return MovementRangeType.HALF;
        }
        else if (_rangeMap[MovementRangeType.FULL].Contains(point))
        {
            return MovementRangeType.FULL;
        }
        else
        {
            return MovementRangeType.NONE;
        }
    }
}
