using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class link to the wood plank
/// </summary>
public class WoodDoor : MonoBehaviour
{
    private string WOOD_PLANK_SOUND = "wood_plank";

    /// <summary>
    /// If the wood plank has been grabbed, kinematic false to throw it on the ground
    /// </summary>
    public void DisableKinematic()
    {
        FindObjectOfType<SoundManager>().Play(WOOD_PLANK_SOUND);
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
}
