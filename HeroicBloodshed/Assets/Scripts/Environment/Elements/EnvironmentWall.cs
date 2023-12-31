using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentWall : MonoBehaviour
{
    private void Awake()
    {
        EnvironmentUtil.AddOutline(this.gameObject, Color.grey, 0.5f);
    }
}
