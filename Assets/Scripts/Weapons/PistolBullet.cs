using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Invoke with the bullet shooted
/// </summary>
public class PistolBullet : MonoBehaviour
{
    private const int MIN_DAMAGE = 18;
    private const int MAX_DAMAGE = 25;
    private const float DELAY_BEFORE_DESTROY = 7.5f;

    private Rigidbody rb;
    private float bulletForce = 12500;
 
    /// <summary>
    /// Get the RigidBody when the bullet go out of the gun
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Add force at every frame to propelling the bullet
    /// </summary>
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * bulletForce * Time.deltaTime);
    }

    /// <summary>
    /// Wait before destroy the bullet after delay
    /// </summary>
    private void Update()
    {
        Destroy(this.gameObject, DELAY_BEFORE_DESTROY);
    }

    /// <summary>
    /// If the bullet touch a collider
    /// </summary>
    /// <param name="other">What the bullet touched</param>
    private void OnTriggerEnter(Collider other)
    {
        // if the bullet touched an Enemy
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyLife>().DecreaseLife(Random.Range(MIN_DAMAGE, MAX_DAMAGE), other.transform.position, other.transform.position.normalized);
            Destroy(this.gameObject);
        }
    }
}
