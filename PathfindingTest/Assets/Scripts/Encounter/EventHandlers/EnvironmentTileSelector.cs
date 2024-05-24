using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileSelector : EncounterEventHandler
{
    private GameObject _quad;

    protected override void OnAwake()
    {
        base.OnAwake();

        CreateTileQuad();
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
        if (_quad == null) return;

        EnvironmentTileRaycastInfo info;

        if(EnvironmentUtil.GetTileBeneathMouse(out info))
        {
            if(info.layer == EnvironmentLayer.CHARACTER || info.layer == EnvironmentLayer.GROUND)
            {
                _quad.transform.position = info.position;
                _quad.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    EncounterStateMachine.OnAbilityChosen.Invoke(AbilityID.MOVE_FULL, info.position);
                }

                return;
            }    
        }

        _quad.SetActive(false);
    }

    private void CreateTileQuad()
    {
        _quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        _quad.transform.parent = this.transform;

        _quad.transform.localEulerAngles = new Vector3(90, 0, 0);
        _quad.transform.localScale = GetTileScale();

        Destroy(_quad.GetComponent<MeshCollider>());

        _quad.layer = LAYER_DECAL;

        MeshRenderer renderer = _quad.GetComponent<MeshRenderer>();

        MaterialLibrary matLib = MaterialLibrary.Get();

        renderer.material = matLib.TileSelector;

        _quad.SetActive(false);
    }
}
