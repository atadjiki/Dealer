using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public virtual void Toggle(bool flag) { }

    public virtual void Reset() { }

    public virtual void Clear() { }

    public virtual void HandleEvent_InteractionContext(InteractableConstants.InteractionContext context) { }

    public virtual void HandleEvent_CharacterSelected(CharacterComponent character) { }

    public virtual void HandleEvent_CharacterDeselected() { }

    public virtual void HandleEvent_SetBehaviorText(AIConstants.BehaviorType behaviorType) { }

    public virtual void HandleEvent_SetAnimText(AnimationConstants.Anim anim) { }

    public virtual void HandleEvent_UpdateBehaviorQueue(Queue<CharacterBehaviorScript> queue) { }

    public virtual void HandleEvent_CharacterLineBegin(Dialogue dialoge) { }

    public virtual void HandleEvent_CharacterLineEnd() { }
}

