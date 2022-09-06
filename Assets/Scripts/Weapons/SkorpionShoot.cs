using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class associate with the Skorpion's gun
/// </summary>
public class SkorpionShoot : MonoBehaviour
{
    private const int NUMBER_OF_BULLET_IN_BURST = 3;
    private const float BURST_DELAY_BETWEEN_BULLETS = 0.05f;
    private const float TIME_AFTER_BURST = 1f;

    public GameObject bullet;

    // Magazine (currently in the gun)
    public PistolMagazine skorpionMagazine;

    public Transform exitPosition;

    //XR
    [SerializeField] XRBaseInteractor xrSocketInteractor;

    /// <summary>
    /// Listen when a magazine has been add or remove
    /// </summary>
    private void Awake()
    {
        xrSocketInteractor.selectEntered.AddListener(AddAmmo);
        xrSocketInteractor.selectExited.AddListener(RemoveMagazine);
    }

    /// <summary>
    /// When the player pull the trigger
    /// </summary>
    public void Shoot()
    {
        if(skorpionMagazine && skorpionMagazine.ammo > 0)
        {
            if (!bullet)
            {
                return;
            }

            FindObjectOfType<SoundManager>().Play("skorpion_shoot");
            StartCoroutine("GustShoot");
            
        }
        else
        {
            FindObjectOfType<SoundManager>().Play("skorpion_empty");
        }
       
    }

    /// <summary>
    /// When the player add a magazine to the gun
    /// </summary>
    /// <param name="args">What has been added to the gun</param>
    public void AddAmmo(SelectEnterEventArgs args)
    {
        skorpionMagazine = args.interactableObject.transform.GetComponent<PistolMagazine>();
        FindObjectOfType<SoundManager>().Play("skorpion_reload");
    }

    /// <summary>
    /// When the player remove the magazine of the gun
    /// </summary>
    /// <param name="args">What has been removed from the gun</param>
    public void RemoveMagazine(SelectExitEventArgs args)
    {
        skorpionMagazine = null;
        FindObjectOfType<SoundManager>().Play("skorpion_reload");
    }

    /// <summary>
    /// Coroutine which will create the burst (throw 3 bullet)
    /// </summary>
    /// <returns></returns>
    private IEnumerator GustShoot()
    {
        for (int i = 0; i < NUMBER_OF_BULLET_IN_BURST; i++)
        {
            yield return new WaitForSeconds(BURST_DELAY_BETWEEN_BULLETS);
            Instantiate(bullet, exitPosition.position, exitPosition.rotation);
            skorpionMagazine.ammo--;
        }
        yield return new WaitForSeconds(TIME_AFTER_BURST);
    }
}
