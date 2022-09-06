using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class will manage the player of different's type of enemy
/// </summary>
public class EnemyLife : MonoBehaviour
{
    public GhoulBehavior ghoulBehavior;
    public WitchBehavior witchBehavior;
    public CrawlerBehavior crawlBehavior;
    public DemonBehavior demonBehavior;

    /// <summary>
    /// When game is started, try to get the script of the enemy
    /// </summary>
    private void Awake()
    {        
        // not a ghoul
        if(!TryGetComponent(out ghoulBehavior))
        {
            ghoulBehavior = null;
        } 

        // not a witch
        if(!TryGetComponent(out witchBehavior))
        {
            witchBehavior = null;
        }

        if(!TryGetComponent(out crawlBehavior))
        {
            crawlBehavior = null;
        }

        if(!TryGetComponent(out demonBehavior))
        {
            demonBehavior = null;
        }
    }

    /// <summary>
    /// When the enemy is touched, called the function to decrease his life
    /// </summary>
    /// <param name="damage">how much damage the enemy will lose</param>
    /// <param name="point">Where the bullet touched the enemy (x;y;z position) (not used for the moment)</param>
    /// <param name="normal">normal of the vector (not used for the moment)</param>
    public void DecreaseLife(int damage, Vector3 point, Vector3 normal)
    {

        if(ghoulBehavior != null)
        {
            ghoulBehavior.DecreaseLife(damage);
            //ghoulBehavior.ShowDamage(point, normal); used previously
        }

        if(witchBehavior != null)
        {
            witchBehavior.DecreaseLife(damage);
           // witchBehavior.ShowDamage(point, normal); used previously
        } 

        if(crawlBehavior != null)
        {
            crawlBehavior.DecreaseLife(damage);
            //showDamage; 
        }

        if(demonBehavior != null)
        {
            demonBehavior.DecreaseLife(damage);
            //showDamage;
        }
    }
}
