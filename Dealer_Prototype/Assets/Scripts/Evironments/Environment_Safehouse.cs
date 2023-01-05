using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    private PlayerComponent _player;

    private SafehouseCanvas _safehouseCanvas;

    [SerializeField] Transform entrace_WalkTo_Location;

    protected override void OnPlayerSpanwed(PlayerComponent playerComponent)
    {
        base.OnPlayerSpanwed(playerComponent);

        _player = playerComponent;

        StartCoroutine(PerformEntranceScene());

    }

    public void OnPlayerDestinationReached()
    {
        if(_safehouseCanvas == null)
        {
            //kick off the safehouse UI
            GameObject safehouseCanvasObject = Instantiate<GameObject>(PrefabLibrary.GetSafehouseCanvas(), this.transform);
            _safehouseCanvas = safehouseCanvasObject.GetComponent<SafehouseCanvas>();
            SafehouseCanvas.OnStationSelected += OnStationSelected;
        }
    }

    private IEnumerator PerformEntranceScene()
    {
        yield return new WaitUntil( () => _player.HasInitialized());

        _player.OnDestinationReached += OnPlayerDestinationReached;

        _player.GoTo(entrace_WalkTo_Location.position);
    }

    private void OnStationSelected(Enumerations.SafehouseStation station)
    {
        Debug.Log("station selected " + station.ToString());
    }

    //private void FixedUpdate()
    //{
    //    if (!_allowInput) return;

    //    if (_player != null && Global.OnMouseContextChanged != null)
    //    {
    //        if (Camera.main != null)
    //        {
    //            Enumerations.MouseContext mouseContext = Enumerations.MouseContext.None;

    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            if (Physics.Raycast(ray, out RaycastHit hit))
    //            {
    //                PlayerStation station = hit.collider.GetComponent<PlayerStation>();

    //                if (station != null)
    //                {
    //                    mouseContext = station.GetMouseContext();
    //                }
    //            }

    //            Global.OnMouseContextChanged.Invoke(mouseContext);           
    //        }
    //    }

    //}
}
