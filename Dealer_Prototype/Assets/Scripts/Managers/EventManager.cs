using System.Collections.Generic;
using UnityEngine;
using Constants;

/*
 * This class acts as the messenger for other singletons, handling things like delegates, etc
 * Other scripts can register to this manager, and will receive events which they can manage on their own end
 * For example, the event manager will receive OnLevelLoaded, then transmit that message to the InputManager
 */

public class EventManager : Singleton<EventManager>
{
    List<IEventReceiver> eventReceivers;

    protected override void Awake()
    {
        base.Awake();

        eventReceivers = new List<IEventReceiver>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void BroadcastEvent(Enumerations.EventID eventID, string info)
    {
        if(debug) Debug.Log("Event Broadcast: " + eventID.ToString() + " " + info);

        foreach (IEventReceiver eventReceiver in eventReceivers)
        {
            eventReceiver.HandleEvent(eventID);
        }
    }

    public void RegisterReceiver(IEventReceiver eventReceiver)
    {
        if (eventReceivers == null || eventReceiver == null) { return; }

        if (eventReceivers.Contains(eventReceiver) == false)
        {
            eventReceivers.Add(eventReceiver);
        }
    }

    public void UnregisterReceiver(IEventReceiver eventReceiver)
    {
        if (eventReceivers == null || eventReceiver == null) { return; }

        if (eventReceivers.Contains(eventReceiver))
        {
            eventReceivers.Remove(eventReceiver);
        }
    }

}
