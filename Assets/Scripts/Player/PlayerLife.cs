using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Class linked to the player's life
/// </summary>
public class PlayerLife : MonoBehaviour
{
    private const float MAX_LIFE_VALUE = 100;
    private const float LIFE_REGEN_SPEED = 7;

    private const string STEP_SOUND = "indoor_steps";
    private const string HIT_SOUND = "player_hit";
    private const string GAMEOVER_SOUND = "gameover";

    private Image img;
    private bool playerInFight = false;
    private bool playerIsDead = false;
    private bool stepsSoundIsStarted = false;

    private float healCooldown = 3f;
    private float maxHealCooldown = 6f;
    private bool startCooldown = false;

    public float playerLife = MAX_LIFE_VALUE;
    public GameObject image;

    public UnityEvent PlayerDeathEvent;
  
    /// <summary>
    /// Get the image of blood when the game is started
    /// </summary>
    private void Awake()
    {
        img = image.GetComponent<Image>();
    }

    /// <summary>
    /// At each frame, change the alpha of the blood image to show to the player if he is near to die.
    /// Heal player if is not in a fight and haven't full life
    /// </summary>
    private void Update()
    {
        var tempColor = img.color;
        
        // Change the alpha of the blood image
        if(playerLife == 100)
        {
            tempColor.a = 0f;
        }
        else
        {
            tempColor.a = 1 - (playerLife / 100);
        }

        img.color = tempColor;   
        
        // if the player can be heal, start the cooldown of the heal
        if(startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if(healCooldown <= 0)
            {
                playerInFight = false;
                startCooldown = false;
            }
        }

        // if the player is not in a fight or dead, heal him following a specific speed
        if (!playerInFight && !playerIsDead)
        {
            if(playerLife <= MAX_LIFE_VALUE - 0.01f)
            {
                playerLife += Time.deltaTime * LIFE_REGEN_SPEED;
            }
            else
            {
                playerLife = MAX_LIFE_VALUE;
                healCooldown = maxHealCooldown;
                playerInFight = true;
            }
        }

        // if player has life under or equal to zero, he is dead
        if(playerLife <= 0)
        {
            playerIsDead = true;
            PlayerDeathEvent.Invoke();
        }
    }


    /// <summary>
    /// Method called when a player is touched by an ennemy
    /// </summary>
    /// <param name="damage">How much damage the player has received</param>
    public void DecreaseLife(int damage)
    {
        if(!playerIsDead)
        {
            FindObjectOfType<SoundManager>().Play(HIT_SOUND);
        }
       
        playerLife = playerLife - damage;
        playerInFight = true;
        healCooldown = maxHealCooldown;
        startCooldown = true;
    }
}
