using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterManager : MonoBehaviour
{
    protected List<CharacterComponent> Characters;
    protected int _popCap = 10;

    protected int _updateEveryFrames = 60 * 3;
    protected int _currentFrames = 0;

    protected virtual void Build()
    {
        Characters = new List<CharacterComponent>();
    }

    public bool HasNotExceededPopCap()
    {
        return Characters.Count < _popCap;
    }

    public abstract bool Register(CharacterComponent Character);

    public abstract void UnRegister(CharacterComponent Character);
}
