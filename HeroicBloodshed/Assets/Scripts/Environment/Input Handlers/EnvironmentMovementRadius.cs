using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentMovementRadius : EnvironmentInputHandler
{
    [SerializeField] private GameObject RadiusObject;

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_GenerateRadiusTiles());
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    private IEnumerator Coroutine_GenerateRadiusTiles()
    {
        EnvironmentInputData inputData = EnvironmentManager.Instance.GetInputData();

        if(inputData.RadiusMaps[MovementRangeType.Half] != null)
        {
            float startTime = Time.time;

            RadiusObject.SetActive(false);

            CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

            List<MeshFilter> childFilters = new List<MeshFilter>();

            //create a quad for each node in the radius map, and add the mesh filter to our list
            foreach (KeyValuePair<Vector3, int> pair in inputData.RadiusMaps[MovementRangeType.Half])
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

                Destroy(quad.GetComponent<BoxCollider>());
 
                quad.transform.position = RadiusObject.transform.InverseTransformPoint(pair.Key);
                quad.transform.eulerAngles = new Vector3(90, 0, 0);
                quad.transform.parent = RadiusObject.transform;

                childFilters.Add(quad.GetComponent<MeshFilter>());
            }

            CombineInstance[] combine = new CombineInstance[childFilters.Count];

            //for each filter, add it to the combine instance
            int i = 0;
            while (i < childFilters.Count)
            {
                combine[i].mesh = childFilters[i].mesh;
                combine[i].transform = childFilters[i].transform.localToWorldMatrix;

                Destroy(childFilters[i].gameObject);

                i++;
            }

            //create our new mesh from the child meshes
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine, true);
            mesh.Optimize();

            //pass the new mesh to the parent mesh filter
            MeshFilter meshFilter = RadiusObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            RadiusObject.SetActive(true);

            Debug.Log("Combined " + i + " meshes to create radius in " + (Time.time-startTime) + " seconds");

            yield return null;
        }
    }
}
