using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static Constants;

public class EnvironmentMovementRadius : EnvironmentInputHandler
{
    [SerializeField] private GameObject Prefab_Tile;

    private List<GameObject> _tiles;

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_GenerateRadiusTiles());
    }

    public override void Deactivate()
    {
        base.Deactivate();

        if(_tiles != null)
        {
            foreach (GameObject tileObject in _tiles)
            {
                GameObject.Destroy(tileObject);
            }

            _tiles.Clear();

            _tiles = null;
        }
    }

    private IEnumerator Coroutine_GenerateRadiusTiles()
    {
        EnvironmentInputData inputData = EnvironmentManager.Instance.GetInputData();

        if(inputData.RadiusMap != null)
        {
            CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

            _tiles = new List<GameObject>();

            foreach (KeyValuePair<Vector3, int> pair in inputData.RadiusMap)
            {
                EnvironmentTileState tileState = EnvironmentTileState.Full;

                if (pair.Value <= currentCharacter.GetMovementRange())
                {

                    tileState = EnvironmentTileState.Half;
                }

                StartCoroutine(Coroutine_GenerateTile(pair.Key, tileState));
            }

            yield return null;
        }
    }

    private IEnumerator Coroutine_GenerateTile(Vector3 position, EnvironmentTileState tileState)
    {
        GameObject tilePrefab = Instantiate(Prefab_Tile, this.transform);

        EnvironmentRadiusTile radiusTile = tilePrefab.GetComponent<EnvironmentRadiusTile>();

        tilePrefab.transform.position = position;

        radiusTile.SetState(tileState);

        _tiles.Add(tilePrefab);

        yield return null;
    }
}
