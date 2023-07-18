using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandgun : CharacterWeapon
{
    [SerializeField] private Transform Transform_Muzzle;

    [SerializeField] private GameObject Prefab_Muzzle_FX;

    [SerializeField] private List<AudioClip> SoundBank_Gunshot;

    public override void OnAttack()
    {
        base.OnAttack();

        PlaySFX();
    }

    private void PlaySFX()
    {
        if(SoundBank_Gunshot.Count > 0)
        {
            AudioSource.PlayClipAtPoint(SoundBank_Gunshot[Random.Range(0, SoundBank_Gunshot.Count - 1)], this.transform.position);
        }
    }
}
