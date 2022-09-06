using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Class link to the crawler enemy
/// </summary>
public class CrawlerBehavior : MonoBehaviour
{
    private const float CHASE_RANGE_VALUE = 20f;
    private const float CRAWLER_SPEED = 1.3f;
    private const float MIN_DAMAGE = 10f;
    private const float MAX_DAMAGE = 15f;
    private const float ATTACK_COOLDOWN = 3f;

    private const string ATTACK_SOUND = "crawler_attack";
    private const string DEATH_SOUND = "crawler_death";
    private const string CRAWLING_SOUND = "crawler_crawling";
  
    private bool canAttack = true;
    private bool lifeShowed = false;
    private bool isDead = false;
    private bool audioStepsIsStarted = false;

    public GameObject player;
    public NavMeshAgent crawlerAgent;

    public float distanceCrawlerPlayer;
    public float chaseRange = CHASE_RANGE_VALUE;
    public float attackRange = 2f;

    public float crawlerLife = 100;
    public float crawlerMaxLife;

    public BoxCollider attackCollider;

    public Animator crawlerAnimator;

   
    public GameObject healthBarUI;
    public Slider slider;
    public bool isChasing = true;

    /// <summary>
    /// When the game is started, "prepare" variables
    /// </summary>
    private void Awake()
    {
        crawlerAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        crawlerAnimator = GetComponent<Animator>();

        crawlerMaxLife = crawlerLife;
        healthBarUI.SetActive(false);
        slider.value = RefreshLife();

        crawlerAgent.speed = CRAWLER_SPEED;
    }

    /// <summary>
    /// At each frame, check the position of the player in the scene and depending it, change the enemy behavior
    /// </summary>
    private void Update()
    {
        // if the enemy has to chase the player
        if(!isChasing)
        {
            chaseRange = 0;
        }
        else
        {
            chaseRange = CHASE_RANGE_VALUE;
        }

        slider.value = RefreshLife();

        // If enemy life = max life, don't show it
        if (crawlerLife < crawlerMaxLife && !lifeShowed)
        {
            healthBarUI.SetActive(true);
            lifeShowed = true;
        }

        // Distane between the player and the enemy
        distanceCrawlerPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceCrawlerPlayer <= crawlerAgent.stoppingDistance)
        {
            RotateToTarget();
        }

        // while the enemy is alive
        if (crawlerLife > 0)
        {
            // if the enemy is in movement, start his movement animation
            if (crawlerAgent.velocity.magnitude > 0)
            {
                crawlerAnimator.SetBool("isCrawling", true);

                if(!audioStepsIsStarted)
                {
                    audioStepsIsStarted = true;
                    FindObjectOfType<SoundManager>().Play(CRAWLING_SOUND);
                }
            }
            else
            {
                if(audioStepsIsStarted)
                {
                    audioStepsIsStarted = false;
                    FindObjectOfType<SoundManager>().Play(CRAWLING_SOUND);
                }
                crawlerAnimator.SetBool("isCrawling", false);
            }

            // depending of his position, change his behavior
            if (distanceCrawlerPlayer > chaseRange)
            {
                Idle();
            }
            if (distanceCrawlerPlayer < chaseRange)
            {
                CrawlToPlayer();
            }
            if (distanceCrawlerPlayer < attackRange)
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
    private void Idle()
    {
        crawlerAgent.velocity = Vector3.zero;
    }

    /// <summary>
    /// When the enemy chase the player
    /// </summary>
    private void CrawlToPlayer()
    {
        RotateToTarget();
        crawlerAgent.SetDestination(player.transform.position);
    }

    /// <summary>
    /// When the enemy attack the player
    /// </summary>
    private void AttackPlayer()
    {
        if(!isDead)
        {
            RotateToTarget();

            crawlerAgent.destination = transform.position;

            Vector3 positionDuringAttack = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(positionDuringAttack);
        }        
    }

    /// <summary>
    /// When the player is dead
    /// </summary>
    private void Death()
    {
        FindObjectOfType<SoundManager>().Play(DEATH_SOUND);
        FindObjectOfType<SoundManager>().Stop(CRAWLING_SOUND);
        isDead = true;
        crawlerAgent.isStopped = true;
        crawlerAnimator.SetBool("isCrawling", false);
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
        return crawlerLife / crawlerMaxLife;
    }

    /// <summary>
    /// When the enemy touch something with his collider
    /// </summary>
    /// <param name="other">Name of the collider which interact with the enemy</param>
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && canAttack)
        {
            FindObjectOfType<SoundManager>().Play(ATTACK_SOUND);
            canAttack = false;
            other.gameObject.SendMessage("DecreaseLife", Random.Range(MIN_DAMAGE, MAX_DAMAGE));
            StartCoroutine("AttackCooldown");
        }
    }

    /// <summary>
    /// Method called when the enemy his touched, to decrease his life
    /// </summary>
    /// <param name="damage">how much life the enemy will loose</param>
    public void DecreaseLife(int damage)
    {
        crawlerLife -= damage;
        chaseRange = 35;
    }

    /// <summary>
    /// Method allowing the enemy to chase the player
    /// </summary>
    public void StartChase()
    {
        isChasing = true;
    }

    /// <summary>
    /// Cooldown between the attacks of the enemy
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(ATTACK_COOLDOWN);
        canAttack = true;
    }
}
