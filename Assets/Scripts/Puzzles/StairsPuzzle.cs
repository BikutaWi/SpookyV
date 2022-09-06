using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class whitch detect if the player placed the cube on the platform
/// </summary>
public class StairsPuzzle : MonoBehaviour
{
    private const float BUTTON_SPEED = 0.5f;
    private const string BEDROOM_KEY_NAME = "BedroomKey";
    private const string STAIRS_SOUND = "stairs_appear";

    private Transform basePos;

    // if the button is goind down or up (cube placed or removed)
    private bool isGoingDown;
    private bool isGoingUp;
    private bool audioHasBeenPlayed = false;


    public GameObject stairsGameObject;

    public Transform button;
    public Transform posDown;
    public Transform posUp;

    public UnityEvent DoorOpeningEvent;
    public UnityEvent DoorClosingEvent;

   
    /// <summary>
    /// When the game is started get the position of the platform to save it
    /// </summary>
    private void Awake()
    {
        button = GetComponentInParent<Transform>();
        basePos = this.transform;

        isGoingDown = false;
        isGoingUp = false;
    }

    /// <summary>
    /// At each frame, move the platform depending of player's actions
    /// </summary>

    private void Update()
    {
        // If the key is on the platform and the platform's still up
        if(isGoingDown)
        {
            while (transform.position.y > posDown.position.y)
            {
                button.transform.Translate(-Vector3.up * BUTTON_SPEED * Time.deltaTime);
                DoorOpeningEvent.Invoke();
            }
            isGoingDown = false;
        }

        // If the key isn't on the platform and the platform's still down
        if(isGoingUp)
        {
            while (transform.position.y < posUp.position.y)
            {
                button.transform.Translate(Vector3.up * BUTTON_SPEED * Time.deltaTime);
                DoorClosingEvent.Invoke();
            }
            isGoingUp = false;
        }
    }

    /// <summary>
    /// When something has been detected by the collider of the platform
    /// </summary>
    /// <param name="other">Collider of the object detected by the platform</param>
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == BEDROOM_KEY_NAME)
        {
            stairsGameObject.SetActive(true);
            isGoingDown = true;
        }

        if(!audioHasBeenPlayed)
        {
            FindObjectOfType<SoundManager>().Play(STAIRS_SOUND);
            audioHasBeenPlayed = true;
        }
    }

    /// <summary>
    /// When something quit the collider of the platform
    /// </summary>
    /// <param name="other">Collider of the object which quit the platform</param>
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == BEDROOM_KEY_NAME)
        {
            isGoingUp = true;
            stairsGameObject.SetActive(false);
        }    
    }
}
