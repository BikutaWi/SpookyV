using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which control the fight in the garden
/// </summary>
public class GardenFight : MonoBehaviour
{
    public UnityEvent EndOfFight;

    public List<GameObject> spawners;
    public int nbEnnemis;

    /// <summary>
    /// When the game is started, get how many spawners and how many ennemies per spawners
    /// </summary>
    private void Awake()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            spawners.Add(child.gameObject);
        }

        if(spawners != null)
        {
            foreach (GameObject child in spawners)
            {
                nbEnnemis += child.GetComponent<MonsterSpawner>().nbEnemiesInZone;
            }
        }
    }

    /// <summary>
    /// At every frame, detect if it's the end of the fight
    /// </summary>
    private void Update()
    {
        if(nbEnnemis <= 0)
        {
            EndOfFight.Invoke();
        }
    }

    /// <summary>
    /// Method get the spawners active (start fight)
    /// </summary>
    public void StartFight()
    {
        foreach(GameObject spawner in spawners)
        {
            spawner.SetActive(true);
        }
    }

    /// <summary>
    /// If an enemy is killed, decrease number of ennemies
    /// </summary>
    public void DecreaseEnemyLeft()
    {
        nbEnnemis--;
    }
}
