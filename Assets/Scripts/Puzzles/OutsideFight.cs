using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which control the fight with the witch
/// </summary>
public class OutsideFight : MonoBehaviour
{
    public UnityEvent WitchDeathEvent;

    private bool eventHasBeenCalled = false;

    /// <summary>
    /// At each frame, detect if the witch is dead and called the event link to her death
    /// </summary>
    private void Update()
    {
        if(transform.childCount == 0 && !eventHasBeenCalled)
        {
            eventHasBeenCalled = true;
            WitchDeathEvent.Invoke();
        }
    }
}
