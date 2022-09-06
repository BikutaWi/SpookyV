using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class link to the keys
/// </summary>
public class KeyBehavior : MonoBehaviour
{
    private const float DELAY_BEFORE_DISABLE_INTERACTABLE = 1.5f;

    /// <summary>
    /// Method called when the key is put on the right door, make it not grabbable anymore
    /// </summary>
   public void NotGrabbableAnymore()
    {
        StartCoroutine("WaitBeforeDisable");
    }

    /// <summary>
    /// Coroutine which wait before disable the grab possibility of the key
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBeforeDisable()
    {
        yield return new WaitForSeconds(DELAY_BEFORE_DISABLE_INTERACTABLE);
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
