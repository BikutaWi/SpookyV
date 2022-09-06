using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which invoke an event every time the player trigger a collider
/// </summary>
public class ColliderEvent : MonoBehaviour
{
    public UnityEvent TriggerEnterEvent;
    
    /// <summary>
    /// Method called when something enter the collider
    /// </summary>
    /// <param name="other">What enter the collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            TriggerEnterEvent.Invoke();
        }
    }
}
