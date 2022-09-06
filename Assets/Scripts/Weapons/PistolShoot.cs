using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class used with the pistol gun
/// </summary>
public class PistolShoot : MonoBehaviour
{
    // simple type
    private float timeBeforeDestroy = 2;
    private float ejectPower = 150f;
    private bool hasGunBeenSlided = true;

    //XR
    [SerializeField] private XRBaseInteractor xrSocketInteractor;

    // Animator
    [SerializeField] private Animator gunAnimator;

    // Transform
    [SerializeField] private Transform casingPosition;
    [SerializeField] private Transform barrelPosition;

    // Gameobject
    public GameObject bullet;
    public GameObject casing;
    public GameObject muzzleFlash;
    public GameObject point;

    // Magazine (currently in the gun)
    public PistolMagazine pistolMagazine;

    public float weaponRange = 100f;

    public float power = 10;

    public LayerMask layerMask;

   
    // not used anymore, was used for shoot RayCast
    // public int minDamage = 1;
    //  public int maxDamage = 5;


    /// <summary>
    /// Method wich will find the animator and the position of the barrel.
    /// </summary>
    void Awake()
    {
        gunAnimator = GetComponent<Animator>();

        if (barrelPosition == null)
        {
            barrelPosition = transform;
        }

        xrSocketInteractor.selectEntered.AddListener(AddAmmo);
        xrSocketInteractor.selectExited.AddListener(RemoveMagazine);
    }

    /// <summary>
    /// Bullet Behaviour when player is shooting. Developped by the creator of the asset
    /// Called by the shoot animation
    /// </summary>
    public void ShootEffect()
    {
        pistolMagazine.ammo--;
        FindObjectOfType<SoundManager>().Play("pistol_shoot");
        if (muzzleFlash)
        {
            //Create the flash effect (Instantiate)
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlash, barrelPosition.position, barrelPosition.rotation);

            //Make the flash disapeared after x seconds
            Destroy(tempFlash, timeBeforeDestroy);
        }
       
        if (!bullet)
        { 
            return; 
        }

       //Instantiate the bullet and throw it in front of the gun
       Instantiate(bullet, barrelPosition.position, barrelPosition.rotation);         
    }

    /// <summary>
    /// Ejection effect on the gun
    /// Called by the shoot animation
    /// </summary>
    void CasingEffect()
    {
        if (!casingPosition || !casing)
        { 
            return; 
        }

        GameObject tempCasing;
        tempCasing = Instantiate(casing, casingPosition.position, casingPosition.rotation) as GameObject;
        
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingPosition.position - casingPosition.right * 0.3f - casingPosition.up * 0.6f), 1f);
        
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, timeBeforeDestroy); 
    }

    /// <summary>
    /// Active when player pull the trigger on the controller. Shoot with the gun
    /// </summary>
    public void Shoot()
    {
        if(pistolMagazine && pistolMagazine.ammo > 0 && hasGunBeenSlided)
        {
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
           FindObjectOfType<SoundManager>().Play("pistol_empty");
        }
        
        
    }


 
    /// <summary>
    /// Method called when the player put a magazine in the gun
    /// </summary>
    /// <param name="args">Which object has been add to the gun</param>
    public void AddAmmo(SelectEnterEventArgs args)
    {
        pistolMagazine = args.interactableObject.transform.GetComponent<PistolMagazine>();
        FindObjectOfType<SoundManager>().Play("pistol_reload");
        hasGunBeenSlided = false;
    }

    /// <summary>
    /// Method called when the player remove the magazine of the gun
    /// </summary>
    /// <param name="args">Which object has been remove of the gun</param>
    public void RemoveMagazine(SelectExitEventArgs args)
    {
        pistolMagazine = null;
        FindObjectOfType<SoundManager>().Play("pistol_reload");
    }

    /// <summary>
    /// Method called when the player slide the upper part of the gun
    /// </summary>
    public void SlideGun()
    {
        hasGunBeenSlided = true;
        FindObjectOfType<SoundManager>().Play("pistol_reload");
    }



    /// <summary>
    /// OBSOLETE VERSION OF THE SHOOT METHOD
    /// Create a Raycast in front of the gun to detect if an enemy is touched
    /// </summary>
    /* private void ShootRayCast()
     {
         RaycastHit hit;
        // Debug.Log("Je tire");

         //V2
          if (Physics.Raycast(point.transform.position, point.transform.TransformDirection(Vector3.forward) * weaponRange, out hit, Mathf.Infinity, layerMask))
         {
             // hit.collider.gameObject.GetComponent<EnemyLife>().DecreaseLife(Random.Range(minDamage, maxDamage), hit.point, hit.normal);
             object[] tempStorage = new object[2];
             tempStorage[0] = hit.point;
             tempStorage[1] = hit.normal;

             hit.collider.SendMessage("DecreaseLife", Random.Range(minDamage, maxDamage));
             hit.collider.SendMessage("ShowDamage", tempStorage);
             // Debug.DrawRay(point.transform.position, point.transform.TransformDirection(Vector3.forward) * weaponRange, Color.blue);

         }      
     } */
}

