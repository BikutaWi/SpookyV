using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class link to the cross in the corridor
/// </summary>
public class CrossPuzzle : MonoBehaviour
{
    private const float CROSS_TURNING_SPEED = 2f;
    private const float ROTATION_VALUE = -90;
    private const string CROSS_SOUND = "fresco_move";

    public UnityEvent CrossDown;

    /// <summary>
    /// If the player click with his pointer on the cross
    /// </summary>
    public void TurnCross()
    {
        // rotate it
        FindObjectOfType<SoundManager>().Play(CROSS_SOUND);
        transform.Rotate(Vector3.forward * ROTATION_VALUE);    

        // if is inverse, called an event
        if (Mathf.Abs(transform.rotation.z) == 1)
        {
            CrossDown.Invoke();
        }
    }


    /// <summary>
    /// When the player resolved the fresco's puzzle, the cross should not be turnable anymore
    /// </summary>
    public void CrossTurnDisable()
    {
        GetComponent<MeshCollider>().isTrigger = true;
        GetComponent<BoxCollider>().isTrigger = true;
    }
}
