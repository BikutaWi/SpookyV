using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum which contain all of the possible position for a door
/// </summary>
public enum MultiplePositions
{
    FullOpenedLeft,
    FullOpenedRight,
    HalfOpenedLeft,
    HalfOpenedRight,
    InverseFullLeft,
    InverseFullRight
}

/// <summary>
/// Class used with the door the player has to open
/// </summary>
public class OpenDoor : MonoBehaviour
{
    private const float DOOR_OPENING_SPEED = 2f;
    private const float DOOR_CLOSING_SPEED = 5f;

    private const string DOOR_OPENING = "door_opening";
    private const string DOOR_CLOSING = "door_closing";

    private float doorPosition;
    private float openedPos;
    public float closedPos;

    public bool hasToOpen;
    public MultiplePositions positionChoice;


    /// <summary>
    /// When the game is started, check which position it can take and it's position when it's close.
    /// </summary>
    private void Awake()
    {
        openedPos = DeterminePosition();
        closedPos = transform.localRotation.eulerAngles.y;
    }

    /// <summary>
    /// At each frame, check if the door has to be open or closed, and move it
    /// </summary>
    private void Update()
    {
        if(hasToOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, openedPos, 0), DOOR_OPENING_SPEED * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, closedPos, 0), DOOR_CLOSING_SPEED * Time.deltaTime);
        }
       
    }

    /// <summary>
    /// Method which determine the angle the door will have when it will be opened
    /// </summary>
    /// <returns>Return the position angle when the door is open</returns>
    private float DeterminePosition()
    {
        switch(positionChoice)
        {
            case MultiplePositions.FullOpenedLeft:
                doorPosition = -90;
                break;
            case MultiplePositions.FullOpenedRight:
                doorPosition = 90;
                break;
            case MultiplePositions.HalfOpenedLeft:
                doorPosition = -40;
                break;
            case MultiplePositions.HalfOpenedRight:
                doorPosition = 40;
                break;
            case MultiplePositions.InverseFullLeft:
                doorPosition = 180;
                break;
            case MultiplePositions.InverseFullRight:
                doorPosition = -180;
                break;
            default:
                doorPosition = -90;
                break;
        }

        return doorPosition;
    }

    /// <summary>
    /// Method which start the movement of the door (to open it)
    /// </summary>
    public void DoorOpening()
    {
        hasToOpen = true;
        FindObjectOfType<SoundManager>().Play(DOOR_OPENING);
    }   

    /// <summary>
    /// Method which start the movement of the door (to close it)
    /// </summary>
    public void DoorClosing()
    {
        hasToOpen = false;
        FindObjectOfType<SoundManager>().Play(DOOR_CLOSING);
    }
}
