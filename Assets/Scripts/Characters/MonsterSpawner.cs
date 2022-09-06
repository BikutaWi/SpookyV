using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Spawner of enemies
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    private const float MIN_TIME_SPAWNER = 5f;
    private const float MAX_TIME_SPAWNER = 10f;

    private int enemiesLeft;


    public int nbEnemiesInZone = 5;

    public GameObject enemyGameObject;

    public Transform spawnPoint;

    public UnityEvent OpenDoor;
    public UnityEvent myEvent;

    /// <summary>
    /// When it's enable, start the spawners
    /// </summary>
    private void Awake()
    {
        spawnPoint = this.transform;
        SpawnerBegin();
    }

    /// <summary>
    /// Start the spawns of the enemies
    /// </summary>
    private void SpawnerBegin()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine("Spawner");
    }

    /// <summary>
    /// Instantiate an enemy in the zone and call InGarden method of the Ghoul
    /// </summary>
    private void SpawnEnemy()
    {
       GameObject newZombie =  Instantiate(enemyGameObject, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity, this.transform);
        newZombie.GetComponent<GhoulBehavior>().InGarden();
    }

    /// <summary>
    /// When all monsters are dead, launch an event
    /// </summary>
    private void SpawnerEnd()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        OpenDoor.Invoke();
    }

    /// <summary>
    /// Timer between 2 enemies spawns
    /// </summary>
    /// <returns>Return the time between 2 spawns</returns>
    private float RandomTimerBetweenSpawn()
    {
        return Random.Range(MIN_TIME_SPAWNER, MAX_TIME_SPAWNER);
    }

   
    /// <summary>
    /// Coroutine which Instanciate enemies following a random timer between spawns
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawner()
    {
        while (nbEnemiesInZone > 0)
        {
            yield return new WaitForSeconds(RandomTimerBetweenSpawn());
            SpawnEnemy();
            nbEnemiesInZone--;
            Debug.Log("Spawn");
        }

        SpawnerEnd();
    }


}
