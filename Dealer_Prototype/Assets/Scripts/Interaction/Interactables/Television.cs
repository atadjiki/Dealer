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
    [SerializeField] private AudioSource AudioSource_TV;
    [SerializeField] private AudioClip AudioClip_On;

    //
    [SerializeField] private Light Light_TV;

    protected override IEnumerator DoInitialize()
    {
        _currentState = DefaultState;

        AudioSource_TV.clip = AudioClip_On;
        AudioSource_TV.loop = true;

        SwitchToState(_currentState);

        Debug.Log("TV initialize");

        yield return base.DoInitialize();
    }

    private void SwitchToState(State _state)
    {
        
        _currentState = _state;

        switch(_state)
        {
            case State.On:
                SetMaterial(Mat_On);
                AudioSource_TV.Play();
                Light_TV.enabled = true;
                break;
            case State.Off:
                SetMaterial(Mat_Off);
                AudioSource_TV.Stop();
                Light_TV.enabled = false;
                break;
        }

        Debug.Log("TV switch to " + _currentState.ToString());
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
