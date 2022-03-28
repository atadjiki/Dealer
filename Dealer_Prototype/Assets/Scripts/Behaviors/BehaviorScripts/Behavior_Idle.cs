using System.Collections;
using Constants;

public class Behavior_Idle : CharacterBehaviorScript
{

    internal override void BeginBehavior()
    {
        base.BeginBehavior();

        _data.Character.SetCurrentBehavior(AIConstants.BehaviorType.Idle);
    }

    protected override IEnumerator Behavior()
    {
        yield return BehaviorHelper.AttemptStand(_data);
        yield return BehaviorHelper.PerformIdle(_data);

        yield return base.Behavior();
    }
}
