using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(EnvironmentWallRaycaster))]
public class CharacterModel : MonoBehaviour, ICharacterEventReceiver
{
    [SerializeField] private GameObject MeshGroup_Main;

    private List<CharacterBodyPartAnchor> _bodyParts;
    private EnvironmentWallRaycaster _wallRaycaster;

    private void Awake()
    {
        _bodyParts = new List<CharacterBodyPartAnchor>(GetComponentsInChildren<CharacterBodyPartAnchor>());
        _wallRaycaster = GetComponent<EnvironmentWallRaycaster>();
    }

    public void ToggleModel(bool flag)
    {
        MeshGroup_Main.SetActive(flag);
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData)
    {
        switch (characterEvent)
        {
            case CharacterEvent.HIT_HARD:
            case CharacterEvent.HIT_LIGHT:
                {
                    SpawnBloodSpray();
                    break;
                }
            case CharacterEvent.DEATH:
                {
                    _wallRaycaster.enabled = false;
                    break;
                }
            default:
                break;
        }
    }

    public Transform GetRandomBodyPart()
    {
        int index = Random.Range(0, _bodyParts.Count);

        return _bodyParts[index].gameObject.transform;
    }

    public Transform GetBodyPart(BodyPartID ID)
    {
        foreach(CharacterBodyPartAnchor bodypart in _bodyParts)
        {
            if(bodypart.GetID() == ID)
            {
                return bodypart.gameObject.transform;
            }
        }

        return this.transform;
    }

    public void SpawnBloodSpray()
    {
        StartCoroutine(Coroutine_ProduceBloodSpray());
    }

    private IEnumerator Coroutine_ProduceBloodSpray()
    {
        ResourceRequest resourceRequest = GetCharacterVFX(PrefabID.VFX_Bloodspray);

        yield return new WaitUntil(() => resourceRequest.isDone);

        GameObject prefab = (GameObject)resourceRequest.asset;

        GameObject particleObject = Instantiate<GameObject>(prefab, GetRandomBodyPart());

        float randomScale = Random.Range(0.5f, 2.0f);

        particleObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        ParticleSystem particleSystem = prefab.GetComponent<ParticleSystem>();

        yield return new WaitForSecondsRealtime(particleSystem.main.duration);

        Destroy(particleObject);
    }

    public bool CanReceiveCharacterEvents()
    {
        return true;
    }
}
