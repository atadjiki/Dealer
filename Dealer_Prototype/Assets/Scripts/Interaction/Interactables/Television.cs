using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
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

    //fmod stuff
    private StudioEventEmitter FMOD_Emitter;

    protected override IEnumerator DoInitialize()
    {
        FMOD_Emitter = GetComponent<StudioEventEmitter>();

        _currentState = DefaultState;

        SwitchToState(_currentState);

        yield return base.DoInitialize();

        Debug.Log("initialzing");
    }

    private void SwitchToState(State _state)
    {

        _currentState = _state;

        switch (_state)
        {
            case State.On:
                SetMaterial(Mat_On);
                Light_TV.enabled = true;
                if (FMOD_Emitter.IsPlaying() == false) FMOD_Emitter.Play();
                FMOD_Emitter.EventInstance.setPaused(false);
                
                break;
            case State.Off:
                SetMaterial(Mat_Off);
                Light_TV.enabled = false;
                FMOD_Emitter.EventInstance.setPaused(true);
                break;
        }
    }

    private void Switch()
    {
        if (_currentState == State.On)
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
