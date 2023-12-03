using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static Constants;

public class EnvironmentMovementRadius : EnvironmentInputHandler
{
    [SerializeField] private GameObject Prefab_RadiusTile;

    private List<GameObject> _generatedTiles;

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_GenerateRadiusTiles());
    }

    public override void Deactivate()
    {
        base.Deactivate();

        if(_generatedTiles != null)
        {
            foreach (GameObject tileObject in _generatedTiles)
            {
                GameObject.Destroy(tileObject);
            }

            _generatedTiles.Clear();

            _generatedTiles = null;
        }
    }

    private IEnumerator Coroutine_GenerateRadiusTiles()
    {
        EnvironmentInputData inputData = EnvironmentManager.Instance.GetInputData();

        if(inputData.RadiusMap != null)
        {
            CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

            _generatedTiles = new List<GameObject>();

            foreach (KeyValuePair<Vector3, int> pair in inputData.RadiusMap)
            {
                EnvironmentTileState tileState;

                if (pair.Value <= currentCharacter.GetMovementRange())
                {

                    tileState = EnvironmentTileState.Half;
                }
                else
                {
                    tileState = EnvironmentTileState.Full;
                }

                StartCoroutine(Coroutine_GenerateTile(pair.Key, tileState));
            }

            yield return null;
        }
    }

    private IEnumerator Coroutine_GenerateTile(Vector3 position, EnvironmentTileState tileState)
    {
        GameObject tilePrefab = Instantiate(Prefab_RadiusTile, this.transform);

        EnvironmentTile environmentTile = tilePrefab.GetComponent<EnvironmentTile>();

        tilePrefab.transform.position = position;

        environmentTile.SetState(tileState);

        _generatedTiles.Add(tilePrefab);

        yield return null;
    }
}
