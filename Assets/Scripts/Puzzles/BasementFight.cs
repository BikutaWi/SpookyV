using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which control the fight in the basement with the boss
/// </summary>
public class BasementFight : MonoBehaviour
{
    private bool eventHasBeenCalled = false;

    public UnityEvent BossDeathEvent;
    
    /// <summary>
    /// At each frame, detect if the boss is dead
    /// </summary>
    private void Update()
    {
        if(transform.childCount == 0 && !eventHasBeenCalled)
        {
            eventHasBeenCalled = true;
            BossDeathEvent.Invoke();
        }
    }
}
