using System;
using System.Collections;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{

    private readonly Vector3 _reticulePos = new Vector3(0.5f, 0.5f, 0);
    private readonly float _damagePopUpOffset = 0.25f;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _weaponFiredClip;
    [SerializeField] private AudioClip _weaponReloadClip;

    [SerializeField] private Transform _laserOrigin;

    Coroutine _shootDelayCoroutine;
    Coroutine _reloadingCoroutine;
    WaitForSeconds _shootDelayTime = new WaitForSeconds(2f);

    private bool _canShoot;
    public bool CanShoot
    {
        get { return _canShoot; }
        private set { _canShoot = value; }  
    }

    private int _damageDealt;
    public int DamageDealt
    {
        get { return _damageDealt; }
        set { _damageDealt = value; }
    }

    // Event - When a Game Object takes damage
    public static Action<int> OnDamageDealt;


    void Start()
    {
        CanShoot = true;
        _shootDelayCoroutine = null;
        _reloadingCoroutine = null;
        _audioSource = GetComponent<AudioSource>();
    }


    // Player shoots -> Player checks if object is an enemy -> Checks if enemy health > 0 -> Trigger event to take damage
    // As this is the script that holds the event, we need to check that we have subscribers before calling the event. Subscribers responsibility to subscribe/desubscribe


    // Called from InputManager
    // Request a bullet from ObjectPoolManager and Shoot.
    public void ShootBullet()
    {
        if (UIManager.Instance.AmmoCount > 0)               // Check that there is a bullet available
        {
            Ray rayOrigin = Camera.main.ViewportPointToRay(_reticulePos);   
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity))           
            {
                StringManager.Instance.SwitchThroughTags(hitInfo);
                ObjectPoolManager.Instance.RequestBullet(hitInfo);      
                UIManager.Instance.UpdateAmmoCount(1);

                // Check if the Game Object hit has the interface IDamageable on it
                if(hitInfo.transform.GetComponent<IDamageable>() != null)
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();     // Store object reference to the script
                    Vector3 _damageSpawnPos = hitInfo.transform.GetChild(0).transform.position;     // All enemies first child is the spawn box 

                    int damageDealt = RandomDamageDealt();
                    FloatingCombatTextPopUp.Instance.InstantiateDamagePopUp(_damageSpawnPos, damageDealt);
                    damageable.ReceiveDamage(damageDealt);
                }
            }
        }
        else
        {
            // Play sound effect to indicate empty ammo and the player needs to reload
            // Add a UI element on the players gameplayer HUD to also visualise this
        }
    }

    //public void ShootDelayTimer()
    //{
    //    if (_shootDelayCoroutine == null)
    //        _shootDelayCoroutine = StartCoroutine(ShootDelayTimerRoutine());                    // Cache coroutine so we can can create a bool null check.
    //    else
    //        return;
    //}

    // Returns a random int value
    int RandomDamageDealt()
    {
        int randomNumber = UnityEngine.Random.Range(20, 41);
        Debug.Log("random value: " + randomNumber); 
        return randomNumber;
    }

    // Coroutine controlling delay time of shooting and audio 
    //IEnumerator ShootDelayTimerRoutine()
    //{
    //    CanShoot = false;
    //    _audioSource.clip = _weaponFiredClip;
    //     _audioSource.Play();
    //    yield return new WaitForSeconds(_weaponFiredClip.length);                  // Can't make changes unless using a separate AudioSource component, OR amend audio clip.
    //    _shootDelayCoroutine = null;
    //    CanShoot = true;
    //}

    // TODO: Call this from somewhere
    // TODO: Add a display message and audio sound to tell the player they need to reload.
    // Play audio sounds for reloading, and once reloaded set the count to 10.
    IEnumerator ReloadingRoutine()
    {
        _audioSource.clip = _weaponReloadClip;          // Assign reloading audio clip
        _audioSource.Play();
        yield return _weaponReloadClip.length;          // Yield until the clip finishes playing
        UIManager.Instance.IsReloading = true;          
        UIManager.Instance.UpdateAmmoCount(10);         // Set Ammo Display count to 10
        _reloadingCoroutine = null;                     // Let the player be able to reload again
    }
}
