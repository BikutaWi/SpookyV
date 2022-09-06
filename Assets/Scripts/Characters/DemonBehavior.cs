using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Class link to the demon enemy (the boss)
/// </summary>
public class DemonBehavior : MonoBehaviour
{
    private const float CHASE_WALK_RANGE_VALUE = 10f;
    private const float CHASE_RUN_RANGE_VALUE = 25f;
    private const float WALK_SPEED = 2.5f;
    private const float RUN_SPEED = 4.5f;
    private const float ATTACK_RANGE = 3f;
    private const float ATTACK_COOLDOWN = 4f;
    private const float HIT_COOLDOWN = 2f;

    private const string WALK_SOUND = "demon_walk";
    private const string DEATH_SOUND = "demon_death";
    private const string SLASH_SOUND = "demon_slash";

    private DemonDamagePlayer demonDamagePlayer;

    private bool lifeShowed = false;
    private bool canAttack = true;
    private float speedBeforeAnimation;
    private bool audioStepsIsPlaying = false;


    public GameObject player;
    public NavMeshAgent demonAgent;

    public float distanceDemonPlayer;
    public float chaseRange = CHASE_WALK_RANGE_VALUE;

    public float demonLife = 350;
    public float maxDemonLife;

    public Animator demonAnimator;

    public GameObject healthBarUI;
    public Slider slider;
  

    /// <summary>
    /// When the game is started, "prepared the variables"
    /// </summary>
    private void Awake()
    {
        demonAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        demonAgent.speed = WALK_SPEED;

        player = GameObject.FindGameObjectWithTag("Player");
        demonAnimator = GetComponent<Animator>();

        demonDamagePlayer = GetComponentInChildren<DemonDamagePlayer>();

        speedBeforeAnimation = demonAgent.speed;

        maxDemonLife = demonLife;
        healthBarUI.SetActive(false);
        slider.value = RefreshLife();
    }

    /// <summary>
    /// At each frame, check the position of the player in the scene and depending it, change the enemy behavior
    /// </summary>
    private void Update()
    {
        slider.value = RefreshLife();

        if (demonLife < maxDemonLife && !lifeShowed)
        {
            healthBarUI.SetActive(true);
            lifeShowed = true;
        }

        // distance betwen player and demon
        distanceDemonPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceDemonPlayer <= demonAgent.stoppingDistance)
        {
            RotateToTarget();
        }


        // while enemy is alive
        if (demonLife > 0)
        {
            // When the enemy is moving
            if (demonAgent.velocity.magnitude > 0 && distanceDemonPlayer < chaseRange && distanceDemonPlayer > ATTACK_RANGE)
            {
               demonAnimator.SetBool("isWalking", true);
                demonAnimator.SetBool("isRunning", false);

                if(!audioStepsIsPlaying)
                {
                    audioStepsIsPlaying = true;
                    FindObjectOfType<SoundManager>().Play(WALK_SOUND);
                }

                demonAgent.speed = WALK_SPEED;
            }
            else if(demonAgent.velocity.magnitude > 0 && distanceDemonPlayer < CHASE_RUN_RANGE_VALUE && distanceDemonPlayer > chaseRange)
            {
                demonAnimator.SetBool("isWalking", false);
                demonAnimator.SetBool("isRunning", true);
                demonAgent.speed = RUN_SPEED;
            }
            else
            {
                if(audioStepsIsPlaying)
                {
                    audioStepsIsPlaying = false;
                    FindObjectOfType<SoundManager>().Stop(WALK_SOUND);
                }

               demonAnimator.SetBool("isWalking", false);
                demonAnimator.SetBool("isRunning", false);
            }

            // if the player is far, patrol
            if (distanceDemonPlayer > CHASE_RUN_RANGE_VALUE)
            {
                Patrol();
            }
            // if the player is not to far, chase him
            if (distanceDemonPlayer < chaseRange && distanceDemonPlayer > ATTACK_RANGE)
            {
                ChasePlayer();
            }
            // if the player is near, attack him
            if (distanceDemonPlayer < ATTACK_RANGE)
            {
                AttackPlayer();
            }
        }
        else
        {
            Death();

        }
    }

    /// <summary>
    /// When the enemy does noting
    /// </summary>
    private void Patrol()
    {
        demonAgent.velocity = Vector3.zero;
    }

    /// <summary>
    /// When the enemy chase the player
    /// </summary>
    private void ChasePlayer()
    {
        RotateToTarget();
        demonAgent.SetDestination(player.transform.position);
    }

    /// <summary>
    /// When the enemy attack the player
    /// </summary>
    private void AttackPlayer()
    {
        RotateToTarget();

        demonAgent.destination = transform.position;

        Vector3 positionDuringAttack = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(positionDuringAttack);

        int animNumber = RandomAnimation(3);

        if (canAttack == true)
        {
            if (animNumber == 1)
            {
                demonAnimator.SetTrigger("Attack1");
            }
            else if (animNumber == 2)
            {

                demonAnimator.SetTrigger("Attack2");
            }

            canAttack = false;
            demonAgent.speed = 0;
            StartCoroutine("AttackCooldown");
        }
        
    }

    /// <summary>
    /// Play the sound of the weapon
    /// </summary>
    public void PlaySlashSound()
    {
        FindObjectOfType<SoundManager>().Play(SLASH_SOUND);
    }

    /// <summary>
    /// When the player is dead
    /// </summary>
    private void Death()
    {
        FindObjectOfType<SoundManager>().Stop(WALK_SOUND);
        FindObjectOfType<SoundManager>().Play(DEATH_SOUND);

        GetComponent<CapsuleCollider>().enabled = false;

        demonAgent.isStopped = true;

        demonAnimator.SetBool("isWalking", false);
        demonAnimator.SetBool("isRunning", false);
        demonAnimator.SetBool("Death", true);

        Destroy(this.gameObject, 10f);
    }

    /// <summary>
    /// Method which turn the enemy gameobject to the player
    /// </summary>
    private void RotateToTarget()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.5f);
    }

    /// <summary>
    /// Refresh the life of the enemy when he's attacked by the player
    /// </summary>
    /// <returns>Return the enemy's current life</returns>
    private float RefreshLife()
    {
        return demonLife / maxDemonLife;
    }

    /// <summary>
    /// Method which pick a random number link to an animation
    /// </summary>
    /// <param name="max">max animation id</param>
    /// <returns>Return the random number</returns>
    private int RandomAnimation(int max)
    {
        return Random.Range(1, max);
    }

    /// <summary>
    /// Decrease the life of the enemy when the player attack him
    /// </summary>
    /// <param name="damage">How much damage the enemy will take</param>
    public void DecreaseLife(int damage)
    {
        demonLife -= damage;

        demonAgent.speed = 0;

        int number = RandomAnimation(15);

        if (number == 1)
        {
            demonAgent.speed = 0;
            demonAnimator.SetTrigger("Hit1");
            StartCoroutine("HitAnimation");
        } 
        else if (number == 2)
        {
            demonAgent.speed = 0;
            demonAnimator.SetTrigger("Hit2");
            StartCoroutine("HitAnimation");
        }
    }

    /// <summary>
    /// Enable the collider of the enemy's weapon.
    /// </summary>
    public void EnableCollider()
    {
        demonDamagePlayer.EnableScyntheCollider();
    }
    
    /// <summary>
    /// Disable the collider of the enemy's weapon.
    /// </summary>
    public void DisableCollider()
    {
        demonDamagePlayer.DisableScyntheCollider();
    }

    /// <summary>
    /// Coroutine which wait before authorize the attacks of the enemy.
    /// </summary>
    /// <returns>Return the seconds before pass the coroutine</returns>
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(ATTACK_COOLDOWN);
        canAttack = true;
        demonAgent.speed = speedBeforeAnimation;
    }

    /// <summary>
    /// Coroutine which wait before re-walk to the player after a hit animation.
    /// </summary>
    /// <returns>Return the seconds before pass the coroutine</returns>
    IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(HIT_COOLDOWN);
        demonAgent.speed = speedBeforeAnimation;
    }



}
