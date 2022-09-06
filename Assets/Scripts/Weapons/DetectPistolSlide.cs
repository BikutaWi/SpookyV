using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which detect if the player have slide the upper part of the gun
/// </summary>
public class DetectPistolSlide : MonoBehaviour
{
    private bool wasAtPosition = false;

    public float threshold = 0.02f;
    public Transform targetPosition;
    public UnityEvent AtPosition;

    /// <summary>
    /// Methods called every frame which check if the upper part has been enough pulled to enable the shoot
    /// </summary>
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, targetPosition.position);

        if(distance < threshold && !wasAtPosition)
        {
            AtPosition.Invoke();
            wasAtPosition = true;
        }

        else if(distance >= threshold)
        {
            wasAtPosition = false;
        }
        
    }
}
