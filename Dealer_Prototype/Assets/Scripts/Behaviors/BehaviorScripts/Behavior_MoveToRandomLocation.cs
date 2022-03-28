using System.Collections;
using Constants;

public class Behavior_MoveToRandomLocation : CharacterBehaviorScript
{
    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(AIConstants.BehaviorType.MoveToRandomLocation);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.AttemptStand(_data);
        yield return BehaviorHelper.PerformMoveToRandomLocation(_data);
        yield return base.Behavior();
    }
}
