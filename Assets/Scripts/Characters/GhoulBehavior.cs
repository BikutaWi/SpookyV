using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Class link to the ghouls enemy
/// </summary>
public class GhoulBehavior : MonoBehaviour
{
    private const float CHASE_RANGE_VALUE = 20f;
    private const float SCREAMING_ANIMATION_TIME = 2f;
    private const float HIT_ANIMATION_TIME = 1.5f;
    private const float ATTACK_COOLDOWN = 3f;

    private const string GHOUL_WALK_SOUND = "ghoul_walk";
    private const string GHOUL_SCREAM_SOUND = "ghoul_scream";
    private const string GHOUL_DEATH_SOUND = "ghoul_death";
    private const string GHOUL_ATTACK_SOUND = "ghoul_attack";

    private float speedBeforeAnimation;
    private bool firstSeen = false;
    private bool lifeShowed = false;
    private bool audioStepsIsPlaying = false;

    public GameObject player;
    public NavMeshAgent ghoulAgent;

    public float distanceGhoulPlayer;
    public float chaseRange = CHASE_RANGE_VALUE;
    public float attackRange = 1.5f;
   
    public float ghoulLife = 100;
    public float ghoulMaxLife;

    public Animator ghoulAnimator;

    public bool isInGarden = false;

    public GameObject healthBarUI;
    public Slider slider;
    
    public GameObject ghoulBloodEffect;

    public bool canAttack = true;

    public GhoulDamagePlayer ghoulDamagePlayer;

    public GameObject GardenZone;
    private bool eventHasBeenCalledOnce = false;

    public bool isChasing = true;

    /// <summary>
    /// When the game is started, "prepared the variables"
    /// </summary>
    void Awake()
    {
        ghoulAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        ghoulAnimator = GetComponent<Animator>();
        ghoulDamagePlayer = GetComponentInChildren<GhoulDamagePlayer>();

        GardenZone = GameObject.FindGameObjectWithTag("GardenZone");

        speedBeforeAnimation  = ghoulAgent.speed;
        ghoulMaxLife = ghoulLife;

        healthBarUI.SetActive(false);
        slider.value = RefreshLife();
                
    }

    /// <summary>
    /// At each frame, check the position of the player in the scene and depending it, change the enemy behavior
    /// </summary>
    void Update()
    {

        // if the ghoul can chase
        if(!isChasing)
        {
            chaseRange = 0;
        }
        else
        {
            chaseRange = CHASE_RANGE_VALUE;
        }

        // refresh enemy's life
        slider.value = RefreshLife();

        if(ghoulLife < ghoulMaxLife && !lifeShowed)
        {
            healthBarUI.SetActive(true);
            lifeShowed = true;
        } 

        // distance betwen player and ghoul
        distanceGhoulPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(distanceGhoulPlayer <= ghoulAgent.stoppingDistance)
        {
            RotateToTarget();
        }


        // if ghoul still alive
        if(ghoulLife > 0)
        {
            // if enemy is in movement
            if(ghoulAgent.velocity.magnitude > 0)
            {
                // if the player has alreay been seen
                if(firstSeen == false)
                {
                    firstSeen = true;
                    PlayerFirstSeen();
                }
                else
                {
                    if(!audioStepsIsPlaying)
                    {
                        audioStepsIsPlaying = true;
                        FindObjectOfType<SoundManager>().Play(GHOUL_WALK_SOUND);
                    }
                    
                    ghoulAnimator.SetBool("isWalking", true);
                }
               
            }
            else
            {
                if(audioStepsIsPlaying)
                {
                    audioStepsIsPlaying = false;
                    FindObjectOfType<SoundManager>().Stop(GHOUL_WALK_SOUND);
                }
                
                 ghoulAnimator.SetBool("isWalking", false);
            }

            // if the player is far, patrol
            if(distanceGhoulPlayer > chaseRange)
            {
                Patrol();
            }
            // if the player is not to far, chase him
            if(distanceGhoulPlayer < chaseRange && distanceGhoulPlayer > attackRange)
            {
                ChasePlayer();
            }
            // if the player is near, attack him
            if(distanceGhoulPlayer < attackRange)
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
    /// When enemy does nothing
    /// </summary>
    private void Patrol()
    {
        ghoulAgent.velocity = Vector3.zero;
        ghoulAnimator.SetBool("isScreaming", false);
    }

    /// <summary>
    /// When enemy chase the player
    /// </summary>
    private void ChasePlayer()
    {
        RotateToTarget();
        ghoulAgent.SetDestination(player.transform.position);

    }

    /// <summary>
    /// When enemy attack the player
    /// </summary>
    private void AttackPlayer()
    {
        RotateToTarget();

        ghoulAgent.destination = transform.position;

        Vector3 positionDuringAttack = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(positionDuringAttack);

        int animNumber = RandomAnimation(3);

        // select random attack between 2 possibilities
        if(canAttack == true)
        {
            FindObjectOfType<SoundManager>().Play(GHOUL_ATTACK_SOUND);
            if (animNumber == 1)
            {
                ghoulAnimator.SetTrigger("attackV1");
            }
            else if (animNumber == 2)
            {

                ghoulAnimator.SetTrigger("attackV2");
            }

            canAttack = false;
            StartCoroutine("AttackCooldown");
        }
       
    }

    /// <summary>
    /// When ghoul is dying
    /// </summary>
    private void Death()
    {
        FindObjectOfType<SoundManager>().Play(GHOUL_DEATH_SOUND);
        FindObjectOfType<SoundManager>().Stop(GHOUL_WALK_SOUND);
        GetComponent<CapsuleCollider>().enabled = false;

        ghoulAgent.isStopped = true;

        ghoulAnimator.SetBool("isWalking", false);

        int animNumber = RandomAnimation(3);

        if(animNumber == 1)
        {
            ghoulAnimator.SetBool("deathV1", true);
        }
        else if(animNumber == 2)
        {
            ghoulAnimator.SetBool("deathV2", true);
        }
        

        if(isInGarden && !eventHasBeenCalledOnce)
        {
            eventHasBeenCalledOnce = true;
            GardenZone.GetComponent<GardenFight>().nbEnnemis--;
        }

        Destroy(this.gameObject, 10f);
    }

    /// <summary>
    /// Look in the direction of the player
    /// </summary>
    private void RotateToTarget()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.5f);
    }

    /// <summary>
    /// Pick a random animation between min and max
    /// </summary>
    /// <param name="max">max animation ID</param>
    /// <returns></returns>
    private int RandomAnimation(int max)
    {
        return Random.Range(1, max);
        
    }

    /// <summary>
    /// If the ghoul see the player for the first time
    /// </summary>
    private void PlayerFirstSeen()
    {
        int animNumber = RandomAnimation(3);
        if (animNumber == 1)
        {
            ghoulAnimator.SetBool("isWalking", true);
            ghoulAnimator.SetBool("isScreaming", false);
        }
        else if (animNumber == 2)
        {
            FindObjectOfType<SoundManager>().Play(GHOUL_SCREAM_SOUND);
            ghoulAnimator.SetBool("isScreaming", true);
            ghoulAgent.speed = 0;
            StartCoroutine("ScreamingAnimation");
            ghoulAnimator.SetBool("isWalking", true);
        }
    }

    /// <summary>
    /// If the enemy lose HP, refresh his lifeBAR
    /// </summary>
    /// <returns>Return current life of the ghoul</returns>
    private float RefreshLife()
    {
        return ghoulLife / ghoulMaxLife;
    }

    /// <summary>
    /// Refresh enemy life when his losing HP and play hit animation sometimes
    /// </summary>
    /// <param name="damage"></param>
    public void DecreaseLife(int damage)
    {
        ghoulLife -= damage;

        // increasae chase range to detect enemy
        chaseRange = 35;

        int number = RandomAnimation(6);

        if(number == 1)
        {
            ghoulAgent.speed = 0;
            ghoulAnimator.SetTrigger("hit");
            StartCoroutine("HitAnimation");
        }
    }


    /// <summary>
    /// Enable collider in hand
    /// </summary>
    public void EnableCollider()
    {
        ghoulDamagePlayer.EnableHandCollider();
    }

    /// <summary>
    /// Disable collider in hand
    /// </summary>
    public void DisableCollider()
    {
        ghoulDamagePlayer.DisableHandCollider();
    }

    /// <summary>
    /// If the enemy is in the garden spawners
    /// </summary>
    public void InGarden()
    {
        isInGarden = true;
    }

    /// <summary>
    /// If the enemey can finally chase the player
    /// </summary>

    public void StartChase()
    {
        isChasing = true;
    }

    /// <summary>
    /// Show blood effect when player shoot on the ghoul, NOT USED FOR THE MOMENT
    /// </summary>
    /// <param name="position">Where the player shoot the enemy</param>
    /// <param name="normal">Normal vector</param>
    /* public void ShowDamage(Vector3 position, Vector3 normal)
     {
         if(ghoulLife > 0)
         {
             // instantiate effect
             GameObject effect = Instantiate(ghoulBloodEffect, position, Quaternion.LookRotation(normal));
             Destroy(effect, 0.5f);
         }   
     } */

    /// <summary>
    /// Coroutine which stop the ghoul movement during screaming animation
    /// </summary>
    /// <returns>Time to wait to pass the coroutine</returns>
    IEnumerator ScreamingAnimation()
    {
        yield return new WaitForSeconds(SCREAMING_ANIMATION_TIME);
        ghoulAgent.speed = speedBeforeAnimation;
    }

    /// <summary>
    /// Coroutone which stop the ghoul movement during Hit animation
    /// </summary>
    /// <returns>Time to wait to pass the coroutine</returns>
    IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(HIT_ANIMATION_TIME);
        ghoulAgent.speed = speedBeforeAnimation;
    }

    /// <summary>
    /// Coroutine which start cooldown between ghoul's attacks
    /// </summary>
    /// <returns>Time to wait to pass the coroutine</returns>
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(ATTACK_COOLDOWN);
        canAttack = true;
    }
}
