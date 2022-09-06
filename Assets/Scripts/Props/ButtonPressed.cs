using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Button which detect when a button is pressed (previous version, StairsPuzzle's Script is the new version of it)
/// </summary>
public class ButtonPressed : MonoBehaviour
{
    private const float NEW_POS_X = 0f;
    private const float NEW_POS_Y = 0.34f;
    private const float NEW_POS_Z = 0f;

    private const string GAMEOBJECT_TAG_NAME = "carton";

    private Vector3 startPosition;
    private Vector3 activatePosition;

    public GameObject buttonGameObject;
    public GameObject door;

    public UnityEvent pressButton;
    public UnityEvent releaseButton;

    public bool isPressed = false;

    /// <summary>
    /// When the game is started, get the position of the button and the position of it when it will be pressed
    /// </summary>
    private void Start()
    {
        startPosition = buttonGameObject.transform.localPosition;
        activatePosition = new Vector3(NEW_POS_X, NEW_POS_Y, NEW_POS_Z);
    }

    /// <summary>
    /// When something enter the collider of the button
    /// </summary>
    /// <param name="other">Collider which touched the button</param>
    private void OnCollisionEnter(Collision other)
    {
        if(!isPressed && other.gameObject.tag == GAMEOBJECT_TAG_NAME)
        {
            buttonGameObject.transform.localPosition = activatePosition;
            this.transform.localPosition = activatePosition;
            pressButton.Invoke();
            isPressed = true;
        }
    }

    /// <summary>
    /// When something exit the collider of the button
    /// </summary>
    /// <param name="other">Collider which exit the button</param>
    private void OnCollisionExit(Collision other)
    {
        if(isPressed && other.gameObject.tag == GAMEOBJECT_TAG_NAME)
        {
            buttonGameObject.transform.localPosition = startPosition;
            releaseButton.Invoke();
            isPressed = false;
        }
    }
}
