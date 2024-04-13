using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnAwake : MonoBehaviour
{
    private void Awake()
    {
        Destroy(this.gameObject);
    }
}
