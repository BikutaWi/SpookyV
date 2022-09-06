using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which called when the gameobject is at a specific position
/// </summary>
public class SwitchPuzzle : MonoBehaviour
{
    private const float POSITION_HAS_TO_BEEN_REACHED = -0.25f;

    private bool hasAlreadyBeenActivated = false;

    public UnityEvent checkPuzzleEvent;

    /// <summary>
    /// If the gameobject reach the position waited
    /// </summary>
    private void LateUpdate()
    {

        if(transform.rotation.z < POSITION_HAS_TO_BEEN_REACHED)
        {
            checkPuzzleEvent.Invoke();
        }
    } 
}
