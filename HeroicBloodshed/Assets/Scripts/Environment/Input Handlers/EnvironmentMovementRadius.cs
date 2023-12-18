using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentMovementRadius : EnvironmentInputHandler
{
    [SerializeField] private Material TileMaterial;

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_GenerateRadiusTiles());
    }

    public override void Deactivate()
    {
        base.Deactivate();

        for(int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator Coroutine_GenerateRadiusTiles()
    {
        EnvironmentInputData inputData = EnvironmentManager.Instance.GetInputData();

        foreach(KeyValuePair<MovementRangeType, Dictionary<Vector3, int>> KeyPair in inputData.RadiusMaps)
        {
            MovementRangeType rangeType = KeyPair.Key;
            Dictionary<Vector3, int> radiusMap = KeyPair.Value;

            Color rangeColor = GetColor(rangeType);
            rangeColor.a = 0.5f;

            if (radiusMap != null)
            {
                float startTime = Time.time;

                GameObject RadiusObject = new GameObject(GetDisplayString(rangeType));
                RadiusObject.transform.parent = this.transform;

                List<Vector3> corners = new List<Vector3>();
                List<Vector3> borderNodes = new List<Vector3>();

                foreach (KeyValuePair<Vector3, int> pair in radiusMap)
                {
                    corners.AddRange(CalculateCorners(pair.Key));
                }

                foreach(Vector3 node in corners)
                {
                    int occurences = 0;

                    foreach(Vector3 comparison in corners)
                    {
                        float distance = Vector3.Distance(node, comparison);

                        if(distance < 0.01f)
                        {
                            occurences++;
                        }
                    }

                    if(occurences <= 2)
                    {
                        borderNodes.Add(node);
                    }
                }

                int count = 0;
                foreach (Vector3 node in borderNodes)
                {
                    if (borderNodes.Contains(node))
                    {
                        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                        quad.name = node.ToString();

                        Destroy(quad.GetComponent<Collider>());
                        quad.transform.parent = RadiusObject.transform;
                        quad.transform.localPosition = RadiusObject.transform.InverseTransformPoint(node);
                        quad.transform.localPosition += new Vector3(0, 0.15f, 0);
                        quad.transform.localEulerAngles = new Vector3(90, 0, 0);
                        quad.transform.localScale = GetTileScaleVector() / 4;

                        MeshRenderer meshRenderer = quad.GetComponent<MeshRenderer>();
                        meshRenderer.material = TileMaterial;
                        meshRenderer.material.color = rangeColor;

                        count++;
                    }
                }

                Debug.Log("created " + count + " quads to create radius in " + (Time.time - startTime) + " seconds");

                yield return null;
            }
        }
    }

    private Vector3[] CalculateCorners(Vector3 center)
    {
        float radius = TILE_SIZE / 2;

        return new Vector3[]
        {
            center + new Vector3(radius, 0, radius),
            center + new Vector3(-radius, 0, radius),
            center + new Vector3(radius, 0, -radius),
            center + new Vector3(-radius, 0, -radius),

        };
    }
}
