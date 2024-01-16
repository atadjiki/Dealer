using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVFX : MonoBehaviour, ICharacterEventReceiver
{
    [Header("Blood")]
    [SerializeField] private List<GameObject> Prefabs_Blood;

    public void HandleEvent(Constants.CharacterEvent characterEvent, object eventData)
    {
        switch(characterEvent)
        {
            case Constants.CharacterEvent.HIT_HARD:
            case Constants.CharacterEvent.HIT_LIGHT:
               // SpawnBloodSpray();
                break;
            default:
                break;
        }
    }

    public void SpawnBloodSpray()
    {
        GameObject prefab = Instantiate<GameObject>(GetRandom(), this.transform);

        //prefab.transform.localPosition = new Vector3(0, 0, 0);
        //prefab.transform.localEulerAngles = new Vector3(0, 90, 0);
        //prefab.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    public GameObject GetRandom()
    {
        return Prefabs_Blood[Random.Range(0, Prefabs_Blood.Count-1)];
    }

    public bool CanReceiveCharacterEvents()
    {
        return true;
    }

}
