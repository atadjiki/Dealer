using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : Interactable
{
    public enum State { Off, On };

    private State _currentState;
    
    [SerializeField] private State DefaultState = State.Off;

    //
    [SerializeField] private MeshRenderer MeshRenderer;

    [SerializeField] private Material Mat_Off;
    [SerializeField] private Material Mat_On;

    //
    [SerializeField] private Light Light_TV;

    protected override IEnumerator DoInitialize()
    {
        _currentState = DefaultState;

        SwitchToState(_currentState);

        yield return base.DoInitialize();
    }

    private void SwitchToState(State _state)
    {
        
        _currentState = _state;

        switch(_state)
        {
            case State.On:
                SetMaterial(Mat_On);
                Light_TV.enabled = true;
                break;
            case State.Off:
                SetMaterial(Mat_Off);
                Light_TV.enabled = false;
                break;
        }
    }

    private void Switch()
    {
        if(_currentState == State.On)
        {
            SwitchToState(State.Off);
        }
        else
        {
            SwitchToState(State.On);
        }

    }

    private void SetMaterial(Material material)
    {
        MeshRenderer.material = material;
    }

    public override void OnInteraction()
    {
        base.OnInteraction();

        Switch();
    }
}
