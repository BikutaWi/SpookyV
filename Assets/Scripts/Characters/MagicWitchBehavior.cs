using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class link to the magic ball of the witch
/// </summary>
public class MagicWitchBehavior : MonoBehaviour
{
    private const float MAGIC_SPEED = 1.5f;
    private const float MAGIC_DAMAGE = 15f;

    private bool hasReducedLife = false;
    private Rigidbody rb;

    private Vector3 positionPlayer;
    private GameObject player;


    public bool chasePlayer = true;
    

    /// <summary>
    /// When the game is started, get the rigidbody, player gameobject and position
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
        positionPlayer = new Vector3(0, player.transform.position.y, -player.transform.position.z - transform.position.z);
    }

    /// <summary>
    /// At each frame, Move the ball to the player
    /// </summary>
    private void Update()
    {
        Destroy(this.gameObject, 2.5f);
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * MAGIC_SPEED);
    }

    /// <summary>
    /// If the player collider interact with something
    /// </summary>
    /// <param name="other">Collider which interact with the ball</param>
    private void OnTriggerEnter(Collider other)
    {
        // if it's the player, reduce his life
        if(other.gameObject.tag == "Player" && !hasReducedLife)
        {
            player.SendMessage("DecreaseLife", MAGIC_DAMAGE);
            hasReducedLife = true;
            Destroy(this.gameObject);
        }

        // if it's a bullet, destroy the magic ball
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
            Debug.Log("Shoot magic");
        }
    }
}
