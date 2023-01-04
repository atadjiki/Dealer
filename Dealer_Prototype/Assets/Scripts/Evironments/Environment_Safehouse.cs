using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    private PlayerComponent _player;

    private bool _allowInput = false;

    [SerializeField] Transform entrace_WalkTo_Location;

    protected override void OnPlayerSpanwed(PlayerComponent playerComponent)
    {
        base.OnPlayerSpanwed(playerComponent);

        _player = playerComponent;

        StartCoroutine(PerformEntranceScene());

    }

    private IEnumerator PerformEntranceScene()
    {
        yield return new WaitUntil( () => _player.HasInitialized());

        _player.OnMovementStateChanged += OnMovementStateChanged;

        yield return new WaitForSeconds(1f);

        _player.GoTo(entrace_WalkTo_Location.position);
    }

    private void OnMovementStateChanged(Enumerations.MovementState state)
    {
        if(state == Enumerations.MovementState.Stopped)
        {
            _player.OnMovementStateChanged -= OnMovementStateChanged;
            _allowInput = true;
        }
    }

    private void FixedUpdate()
    {
        if (!_allowInput) return;

        if (_player != null && Global.OnMouseContextChanged != null)
        {
            if (Camera.main != null)
            {
                Enumerations.MouseContext mouseContext = Enumerations.MouseContext.None;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    PlayerStation station = hit.collider.GetComponent<PlayerStation>();

                    if (station != null)
                    {
                        mouseContext = station.GetMouseContext();
                    }
                }

                Global.OnMouseContextChanged.Invoke(mouseContext);           
            }
        }

    }
}
