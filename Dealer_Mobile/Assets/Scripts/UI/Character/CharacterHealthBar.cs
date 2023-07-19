using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_HealthBarPiece;
    [Space]
    [Header("Debug")]
    [SerializeField] private bool _debug;
    [SerializeField] private int _debug_health;

    private void Awake()
    {
        if(_debug)
        {
            Setup(_debug_health);
        }
    }

    public void Setup(int healthPoints)
    {

        for(int i = 0; i < healthPoints; i++)
        {
            GameObject healthBarPiece = Instantiate<GameObject>(Prefab_HealthBarPiece, this.transform);
        }
    }
}
