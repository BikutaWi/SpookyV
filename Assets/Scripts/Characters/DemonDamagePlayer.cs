using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class link to the weapon of the demon
/// </summary>
public class DemonDamagePlayer : MonoBehaviour
{
    private const int MIN_DAMAGE = 4;
    private const int MAX_DAMAGE = 8;

    public BoxCollider scyntheCollider;

    /// <summary>
    /// When the game is started, get the Collider of the weapon
    /// </summary>
    private void Awake()
    {
        scyntheCollider = GetComponent<BoxCollider>();
        scyntheCollider.enabled = false;
    }

    /// <summary>
    /// When something interact with the collider of the weapon
    /// </summary>
    /// <param name="other">collider of the object entered the weapon</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("DecreaseLife", Random.Range(MIN_DAMAGE, MAX_DAMAGE));
        }
    }

    /// <summary>
    /// Enable the weapon collider 
    /// </summary>
    public void EnableScyntheCollider()
    {
        scyntheCollider.enabled = true;
        Debug.Log("Enable");
    }

    /// <summary>
    /// Disable the weapon collider
    /// </summary>
    public void DisableScyntheCollider()
    {
        scyntheCollider.enabled = false;
        Debug.Log("Disable");
    } 
}
