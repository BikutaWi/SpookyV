using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class used to get the hand's ActionBasedController component
/// </summary>

public class DetectHandMovement : MonoBehaviour
{
    public ActionBasedController device;

    private void Awake()
    {
        device = GetComponent<ActionBasedController>();         
    }
}
