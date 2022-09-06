using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which add gravity to the player
/// </summary>
public class PlayerGravity : MonoBehaviour
{
    private CharacterController characterController;


    /// <summary>
    /// Get the CharacterController component when the game is started
    /// </summary>
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// At each frame if the player don't touch the ground, add Vector 3 movement down
    /// </summary>
    void Update()
    {
        characterController.SimpleMove(Vector3.forward * 0);
    }
}
