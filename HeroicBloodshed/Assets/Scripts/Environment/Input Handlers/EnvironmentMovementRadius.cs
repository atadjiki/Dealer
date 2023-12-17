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
            rangeColor.a = 0.2f;

            if (radiusMap != null)
            {
                float startTime = Time.time;

                //create a child object to house the radius
                GameObject RadiusObject = new GameObject(GetDisplayString(rangeType));
                RadiusObject.transform.parent = this.transform;

                int count = 0;
                foreach (KeyValuePair<Vector3, int> pair in radiusMap)
                {
                    Vector3 nodePosition = pair.Key;

                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

                    Destroy(quad.GetComponent<Collider>());
                    quad.transform.parent = RadiusObject.transform;
                    quad.transform.localPosition = RadiusObject.transform.InverseTransformPoint(nodePosition);
                    quad.transform.localPosition += new Vector3(0, 0.15f, 0);
                    quad.transform.localEulerAngles = new Vector3(90, 0, 0);
                    quad.transform.localScale = GetTileScaleVector();

                    MeshRenderer meshRenderer = quad.GetComponent<MeshRenderer>();
                    meshRenderer.material = TileMaterial;
                    meshRenderer.material.color = rangeColor;

                    count++;
                }

                Debug.Log("created " + count + " quads to create radius in " + (Time.time - startTime) + " seconds");

                yield return null;
            }
        }
    }
}
