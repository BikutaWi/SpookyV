using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class linked to the fountain in front of the house
/// </summary>
public class FountainBehavior : MonoBehaviour
{
    private const float MAX_WATER_LEVEL = 0.2f;
    private const float WATER_SPEED = 0.15f;
    private const string FOUNTAIN_KEY_NAME = "KeyFountain";
    private const string WATER_SOUND = "water";

    private bool hasBeenOpenedOnce = false;
    private bool audioHasBeenStopped = false;

    public UnityEvent KeyInside;

    public GameObject waterFX;

    /// <summary>
    /// At each frame, raise up the water level if the key is inside
    /// </summary>
    private void Update()
    {
        if(waterFX.activeSelf && waterFX.transform.localPosition.y < MAX_WATER_LEVEL)
        {
            waterFX.transform.Translate(Vector3.up * WATER_SPEED * Time.deltaTime);
        }
    }

    /// <summary>
    /// If something has been throwed in the water
    /// </summary>
    /// <param name="other">Collider of the object throwed</param>
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == FOUNTAIN_KEY_NAME && !hasBeenOpenedOnce)
        {
            hasBeenOpenedOnce = true;
            KeyInside.Invoke();

            other.gameObject.GetComponentInParent<XRGrabInteractable>().enabled = false;

            WaterLevel();
        }
    }

    

    /// <summary>
    /// Method called when the water has to raise
    /// </summary>
    public void WaterLevel()
    {
        FindObjectOfType<SoundManager>().Play(WATER_SOUND);
        waterFX.SetActive(true);
    }
}
