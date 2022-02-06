using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCComponent : CharacterComponent
{ 

    internal override void Initialize(SpawnData spawnData)
    {
        base.Initialize(spawnData);

        if (NPCManager.Instance.Register(this) == false)
        {
            Destroy(this.gameObject);
        }
    }

    internal override IEnumerator DoInitialize()
    {
        yield return base.DoInitialize();

        //register camera
        CharacterCameraManager.Instance.RegisterCharacterCamera(this);

        //ready to begin behaviors
        updateState = CharacterConstants.UpdateState.Ready; //let the manager know we're ready to be handled

        CharacterMode = spawnData.GetMode();

        yield return null;
    }

    private void OnDestroy()
    {
        NPCManager.Instance.UnRegister(this);
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);
    }

}
