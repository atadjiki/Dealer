using System.Collections;
using Constants;

public class Behavior_MoveToLocation : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(AIConstants.BehaviorType.MoveToLocation);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.AttemptStand(_data);
        yield return BehaviorHelper.PerformMoveToDestination(_data);

        yield return base.Behavior();
    }
}
