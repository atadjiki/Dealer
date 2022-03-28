using System.Collections;
using Constants;

public class Behavior_ApproachInteractable : CharacterBehaviorScript
{

    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(AIConstants.BehaviorType.Approach_Interactable);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.AttemptStand(_data);
        yield return BehaviorHelper.PerformApproachInteractable(_data);
        yield return BehaviorHelper.ResolvePerformInteraction(_data);
        yield return base.Behavior();
    }
}
