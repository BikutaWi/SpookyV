using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Enum of objects grabbable in the kitchen
/// </summary>
public enum Items
{
    Can,
    Bowl
}

/// <summary>
/// Class which detect if an item is poperly tidy in the kitchen
/// </summary>
public class KitchenItems : MonoBehaviour
{
    private const float WAIT_BEFORE_STOP_COLLISIONS = 3f;

    private bool canCollide = false;

    public Items items;
    public UnityEvent itemOnPos;
    
    /// <summary>
    /// Method called when an item an been put in the kitchen
    /// </summary>
    /// <param name="other"></param>
     private void OnTriggerStay(Collider other)
     {
        
        if(canCollide)
        {
            switch (items)
            {
                case Items.Bowl:
                    BowlOnPos(other);
                    break;
                case Items.Can:
                    CanOnPos(other);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Method called When the objects put is a bowl
    /// </summary>
    /// <param name="gameobject">collider of the object which has been placed</param>
    private void BowlOnPos(Collider gameobject)
    {
        if (gameobject.tag == Items.Bowl.ToString())
        {
            itemOnPos.Invoke();
            CollideStatus();
            StartCoroutine("WaitBeforeStopCollisions");
            gameobject.GetComponent<Rigidbody>().detectCollisions = false;

        }
    }

    /// <summary>
    /// Method called when the object put is a can
    /// </summary>
    /// <param name="gameobject">collider of the object which has been placed</param>
    private void CanOnPos(Collider gameobject)
    {
        if (gameobject.tag == Items.Can.ToString())
        {
            itemOnPos.Invoke();
            CollideStatus();
            StartCoroutine("WaitBeforeStopCollisions");
            gameobject.GetComponent<Rigidbody>().detectCollisions = false;
        }
    }

    /// <summary>
    /// Change if a place can collide
    /// </summary>
    public void CollideStatus()
    {
        canCollide = !canCollide;
    }


    /// <summary>
    /// Coroutine which count a delay before stop the collision between the place and the gameobject
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBeforeStopCollisions()
    {
        yield return new WaitForSeconds(WAIT_BEFORE_STOP_COLLISIONS);
    }
}

