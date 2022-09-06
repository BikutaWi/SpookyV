using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WitchBehavior : MonoBehaviour
{
    private const int RADIUS_TELEPORTATION = 25;
    private const float TELEPORT_RANGE = 5f;
    private const float ATTACK_COOLDOWN = 3f;
    private const float HIT_ANIMATION_TIMER = 2f;
    private const float TELEPORT_COOLDOWN = 5f;

    private const string WITCH_ATTACK_SOUND = "witch_attack";
    private const string WITCH_DEATH_SOUND = "witch_death";
    private const string WITCH_TELEPORT_SOUND = "witch_teleport";
    private const string WITCH_LAUGH_SOUND = "witch_laugh";

    private Animator witchAnimator;
    private bool lifeShowed = false;
    private bool canTeleport = true;
    private float attack_range = 30f;

    public float distancePlayerWitch;
    public float witchLife = 100f;
    public float witchMaxLife;
    public float speedBeforeAnimation;

    public bool canAttack = true;

    public Transform magicSpawner;

    public NavMeshAgent witchAgent;

    public GameObject player;
    public GameObject HealthBarUI;
    public GameObject bloodEffect;
    public GameObject magic;

    public Slider slider;

    /// <summary>
    /// When Gameobject setActive(true), play laughing sound
    /// </summary>
    private void OnEnable()
    {
        FindObjectOfType<SoundManager>().Play(WITCH_LAUGH_SOUND);
    }

    /// <summary>
    /// When the game is started, "prepared the variables"
    /// </summary>
    private void Awake()
    {
        witchAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        witchAnimator = GetComponent<Animator>();

        witchMaxLife = witchLife;

        speedBeforeAnimation = witchAgent.speed;

        HealthBarUI.SetActive(false);
        slider.value = RefreshLife();

    }

    /// <summary>
    /// At each frame, check the position of the player in the scene and depending it, change the enemy behavior
    /// </summary>
    void Update()
    {
        slider.value = RefreshLife();

        if(witchLife < witchMaxLife && !lifeShowed)
        {
            HealthBarUI.SetActive(true);
            lifeShowed = true;
        }

        // distance between witch and player
        distancePlayerWitch = Vector3.Distance(player.transform.position, transform.position);

        // if witch still alive
        if (witchLife > 0)
        {
            // if witch in movement
            if (witchAgent.velocity.magnitude > 0)
            {
               witchAnimator.SetBool("isRunning", true);
            }
            else
            {
                witchAnimator.SetBool("isRunning", false);
            }

            // if player is far, patrol
            if (distancePlayerWitch > attack_range)
            {
                Patrol();
            }
            
            //if player is not to far, throw magic
            if (distancePlayerWitch < attack_range)
            {
                ThrowMagic();
            }

            // if player is near, teleport
            if(distancePlayerWitch < TELEPORT_RANGE && canTeleport)
            {
                witchAnimator.SetTrigger("teleport");
                
            }
            else if(distancePlayerWitch < TELEPORT_RANGE && !canTeleport)
            {
                ThrowMagic();
            }
        }
        else
        {
            Death();
        }
    }

    /// <summary>
    /// When enemy does nothing
    /// </summary>
    private void Patrol()
    {
        witchAgent.velocity = Vector3.zero;
        RotateToTarget();
    }

    /// <summary>
    /// When enemy throw magic to the player
    /// </summary>
    private void ThrowMagic()
    {
        witchAgent.SetDestination(this.transform.position);

        RotateToTarget();
        int animNumber = RandomAnimation(3);

        // 2 animation possibilities
        if(canAttack)
        {
            FindObjectOfType<SoundManager>().Play(WITCH_ATTACK_SOUND);
            if (animNumber == 1)
            {
                witchAnimator.SetTrigger("attack_v1");
            }
            else if (animNumber == 2)
            {
                witchAnimator.SetTrigger("attack_v2");
            }

            canAttack = false;
            StartCoroutine("AttackCooldown");
        }
    }

    /// <summary>
    /// Instantiate the magic ball
    /// </summary>
    public void Magic()
    {
        Instantiate(magic, magicSpawner.position, transform.rotation);
    }

    /// <summary>
    /// Teleport when the player is to near
    /// </summary>
    public void Teleport()
    {
        if(canTeleport)
        {
            FindObjectOfType<SoundManager>().Play(WITCH_TELEPORT_SOUND);
            witchAgent.speed = 0;
            Vector3 newPosition = Random.insideUnitCircle * RADIUS_TELEPORTATION;
            newPosition.x += transform.position.x;
            newPosition.z = newPosition.y + transform.position.z;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            canTeleport = false;
            StartCoroutine("TeleportCooldown");
        }
    }

    /// <summary>
    /// Rotate to the target (Player)
    /// </summary>
    private void RotateToTarget()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position);

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

       transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }

    /// <summary>
    /// When witch's life <= 0, she's dead
    /// </summary>
    private void Death()
    {
        FindObjectOfType<SoundManager>().Play(WITCH_DEATH_SOUND);
        witchAgent.isStopped = true;

        int animNumber = RandomAnimation(3);

        if (animNumber == 1)
        {
            witchAnimator.SetBool("death_V1", true);
        }
        else if (animNumber == 2)
        {
            witchAnimator.SetBool("death_V2", true);
        }

        Destroy(this.gameObject, 10f);
    }

    /// <summary>
    /// When the player touch the enemy, reduce his life
    /// </summary>
    /// <param name="damage">How much damage the witch will take</param>
    public void DecreaseLife(int damage)
    {
        witchLife -= damage;

        attack_range = 35f;

        int number = RandomAnimation(4);

        // 1/3 chance to start hit animation
        if(number == 3)
        {
            witchAgent.speed = 0;
            witchAnimator.SetTrigger("hit");
            StartCoroutine("HitAnimation");
        } 
    }


    /// <summary>
    /// Pick a random animation ID between 1 and MAX ID
    /// </summary>
    /// <param name="maxNb">Max ID animation</param>
    /// <returns>Return a number which determine which animation to play</returns>
    private int RandomAnimation(int maxNb)
    {
        return Random.Range(1, maxNb);
    }

    /// <summary>
    /// When player touch the enemy, refresh his lifebar
    /// </summary>
    /// <returns>Return the current life to print on the lifebar</returns>
    private float RefreshLife()
    {
        return witchLife / witchMaxLife;
    }

    /// <summary>
    /// SHow the blood damage where the player shoot on the enemy, not used for now but still work
    /// </summary>
    /// <param name="position">Where the player touch the enemy</param>
    /// <param name="normal">Normal vector</param>
    /* public void ShowDamage(Vector3 position, Vector3 normal)
     {
         if (witchLife > 0)
         {
             GameObject effect = Instantiate(bloodEffect, position, Quaternion.LookRotation(normal));
             Destroy(effect, 0.5f);
         }
     } */

    /// <summary>
    /// Coroutine which start cooldown between Attacks
    /// </summary>
    /// <returns>Return how may seconds to wait to pass the timer</returns>
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(ATTACK_COOLDOWN);
        canAttack = true;
    }

    /// <summary>
    /// Coroutone which stop the witch movement during Hit animation
    /// </summary>
    /// <returns>Time to wait to pass the coroutine</returns>
    IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(HIT_ANIMATION_TIMER);
        witchAgent.speed = speedBeforeAnimation;
    }

    /// <summary>
    /// Coroutone which stop the witch movement during teleport animation
    /// </summary>
    /// <returns>Time to wait to pass the coroutine</returns>
    IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(TELEPORT_COOLDOWN);
        canTeleport = true;
        witchAgent.speed = speedBeforeAnimation;
    }
}
