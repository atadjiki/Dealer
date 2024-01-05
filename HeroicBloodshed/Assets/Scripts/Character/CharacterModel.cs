using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterModel : MonoBehaviour, ICharacterEventReceiver
{
    [SerializeField] private GameObject MeshGroup_Main;

    private CapsuleCollider _collider;

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
            default:
                break;
        }
    }

    public bool CanReceiveCharacterEvents()
    {
        return true;
    }

    private void SpawnBloodSpray()
    {
        StartCoroutine(Coroutine_ProduceBloodSpray(this.transform));
    }

    private IEnumerator Coroutine_ProduceBloodSpray(Transform parentTransform)
    {
        ResourceRequest resourceRequest = GetCharacterVFX(PrefabID.VFX_Bloodspray);

        yield return new WaitUntil(() => resourceRequest.isDone);

        GameObject prefab = (GameObject)resourceRequest.asset;

        GameObject particleObject = Instantiate<GameObject>(prefab, parentTransform);

        float randomScale = Random.Range(0.5f, 2.0f);

        particleObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        ParticleSystem particleSystem = prefab.GetComponent<ParticleSystem>();

        yield return new WaitForSecondsRealtime(particleSystem.main.duration);

        Destroy(particleObject);
    }
}
