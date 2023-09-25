using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public interface ICharacterEventReceiver
{
    public abstract void HandleEvent(object eventData, CharacterEvent characterEvent);
}
