using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public interface ICharacterEventHandler
{
    public abstract void HandleEvent(CharacterEvent characterEvent, object eventData);
}