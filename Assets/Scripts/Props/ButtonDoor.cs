using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class detect if the player's hand touched the button associated
/// </summary>
public class ButtonDoor : MonoBehaviour
{
    private const float POSITION_PRESSED_X = -0.6f;
    private const float POSITION_PRESSED_Y = -0.25f;
    private const float POSITION_PRESSED_Z = 0.47f;

    private const string LEFT_HAND_PLAYER_NAME = "left_hand";
    private const string RIGHT_HAND_PLAYER_NAME = "right_hand";
    private const string BUTTON_CLICK_SOUND = "button_click";

    private bool hasBeenPressedOnce = false;
    private Vector3 positionBeforePress;
    private Vector3 positionAfterPress;

    public UnityEvent pressButton;
    public GameObject button;

    /// <summary>
    /// Get the position of the button and after pressing it
    /// </summary>
    private void Awake()
    {
        positionBeforePress = button.transform.localPosition;
        positionAfterPress = new Vector3(POSITION_PRESSED_X, POSITION_PRESSED_Y, POSITION_PRESSED_Z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == LEFT_HAND_PLAYER_NAME || other.gameObject.tag == RIGHT_HAND_PLAYER_NAME)
        {
            button.transform.localPosition = positionAfterPress;
            FindObjectOfType<SoundManager>().Play(BUTTON_CLICK_SOUND);
            if (!hasBeenPressedOnce)
            {
                hasBeenPressedOnce = true;
                pressButton.Invoke(); 
            }
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<SoundManager>().Play(BUTTON_CLICK_SOUND);
        button.transform.localPosition = positionBeforePress;
    }
}
