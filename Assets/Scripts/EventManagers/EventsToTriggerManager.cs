using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsToTriggerManager : Singleton<EventsToTriggerManager>
{
    public static event Action <EventsToTrigger> OnTriggerEvent;

    public static void AlertEventTriggered(EventsToTrigger e)
    {
        OnTriggerEvent?.Invoke(e);
        //switch(e)
        //{
        //    case EventsToTrigger.YourPokemonSwitched:
                
        //        break;
        //}
    }
}
