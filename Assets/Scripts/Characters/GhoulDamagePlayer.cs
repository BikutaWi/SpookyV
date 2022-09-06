using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class link to the hand of the ghouls
/// </summary>
public class GhoulDamagePlayer : MonoBehaviour
{
    private const float MIN_DAMAGE = 5f;
    private const float MAX_DAMAGE = 10f;

    public BoxCollider handCollider;

    /// <summary>
    /// When the game is started, get the BoxCollider of the hand and disable it
    /// </summary>
    private void Awake()
    {
        handCollider = GetComponent<BoxCollider>();
        handCollider.enabled = false;
    }

    /// <summary>
    /// If the collider of the hand detect the player, reduce the player's life
    /// </summary>
    /// <param name="other">Collider which interact with the hand</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("DecreaseLife", Random.Range(MIN_DAMAGE,MAX_DAMAGE));
        }
    }

    /// <summary>
    /// Enable the collider of the hand
    /// </summary>
    public void EnableHandCollider()
    {
        handCollider.enabled = true;
    }

    /// <summary>
    /// Disable the collider of the hand
    /// </summary>
    public void DisableHandCollider()
    {
        handCollider.enabled = false;
    }
}

