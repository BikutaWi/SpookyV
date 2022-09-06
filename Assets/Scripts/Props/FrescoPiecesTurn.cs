using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class link to the pieces of the fresco in the room
/// </summary>
public class FrescoPiecesTurn : MonoBehaviour
{
    private const int ROTATION_VALUE = 90;
    private const string FRESCO_SOUND = "fresco_move";

    /// <summary>
    /// Method called when the player click whith his controller on a piece
    /// </summary>
    public void RotatePieces()
    {
        FindObjectOfType<SoundManager>().Play(FRESCO_SOUND);
       transform.Rotate(Vector3.right * ROTATION_VALUE);           
    }
}
