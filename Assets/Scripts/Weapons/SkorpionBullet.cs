using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class called when a bullet is propellig from the skorpion's gun
/// </summary>
public class SkorpionBullet : MonoBehaviour
{
    private const float DELAY_BEFORE_DESTROY = 6.5f;
    private int MIN_DAMAGE = 5;
    private int MAX_DAMAGE = 8;
    private float BULLET_FORCE = 10000;

    private EnemyLife enemyLife;

    private Rigidbody rb;



    /// <summary>
    /// Get the RigidBody when the bullet is instancied
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Add force to the bullet at every frame
    /// </summary>
    private void FixedUpdate()
    {
        rb.AddForce(-transform.forward * BULLET_FORCE * Time.deltaTime);
    }

    /// <summary>
    /// Count delay before destroy the bullet
    /// </summary>
    private void Update()
    {
        Destroy(this.gameObject, DELAY_BEFORE_DESTROY);
    }


    /// <summary>
    /// When the bullet detect another collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyLife>().DecreaseLife(Random.Range(MIN_DAMAGE, MAX_DAMAGE), other.transform.position, other.transform.position.normalized);
            Destroy(this.gameObject);
        }
    }
}
