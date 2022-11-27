using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameplayInitializer
{
    public void Initialize();

    public IEnumerator PerformInitialize();

    public bool HasInitialized();
}
