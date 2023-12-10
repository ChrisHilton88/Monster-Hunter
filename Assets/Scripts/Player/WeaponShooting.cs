using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    private int _minDiceRoll = 0, _maxDiceRoll = 101; 

    private readonly Vector3 _reticulePos = new Vector3(0.5f, 0.5f, 0);

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _weaponFiredClip;
    [SerializeField] private AudioClip _weaponReloadClip;
    [SerializeField] private AudioClip _emptyAmmoClip;

    [SerializeField] private Transform _laserOrigin;

    private Coroutine _shootDelayCoroutine;
    private Coroutine _reloadingCoroutine;
    private WaitForSeconds _shootDelayTime = new WaitForSeconds(2f);

    public static Action OnShootWeapon;      // Event that is responsible for passing ammo when shooting a weapon 
    public static Action OnReloadWeapon;     // Event that is responsible for reloading ammo in current weapon
    public static Action OnEmptyClip;        // Event that is responsible for managing audio/UI when Ammo clip is empty  



    #region Initialisation
    void OnEnable()
    {
        OnShootWeapon += ShootBullet;
        OnEmptyClip += EmptyAmmoClip;
    }

    void OnDisable()
    {
        OnShootWeapon -= ShootBullet;
        OnEmptyClip += EmptyAmmoClip;
    }

    void Start()
    {
        _shootDelayCoroutine = null;
        _reloadingCoroutine = null;
        _audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Methods
    public void ShootBullet()
    {
        if (Ammo.Instance.CurrentAmmoCount > 0)               // Check that there is a bullet available
        {
            _audioSource.clip = _weaponFiredClip;
            _audioSource.volume = 0.5f;
            _audioSource.Play();    

            Ray rayOrigin = Camera.main.ViewportPointToRay(_reticulePos);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity))
            {
                StringManager.Instance.SwitchThroughTags(hitInfo);
                BulletObjectPool.Instance.RequestBullet(hitInfo);

                if (hitInfo.transform.GetComponent<IDamageable>() != null)       // Check if GameObject has IDamageable
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    Vector3 _damageSpawnPos = hitInfo.transform.GetChild(0).transform.position;     // All gameobjects with IDamageable first child is the spawn box

                    int damageDealt = RandomDamageDealt();
                    FloatingCombatTextPopUp.Instance.ActivateDamagePopUp(_damageSpawnPos, damageDealt);
                    damageable.ReceiveDamage(damageDealt);
                }
                else
                    Debug.Log("Hit target IDamageable is NULL - WeaponShooting class");
            }
        }
        else
            OnEmptyClip?.Invoke();          // Ammo must be empty
    }
    #endregion

    #region Events
    private void EmptyAmmoClip()
    {
        _audioSource.clip = _emptyAmmoClip;
        _audioSource.volume = 1f;
        _audioSource.Play();
    }

    public void TriggerWeaponShootingEvent()
    {
        OnShootWeapon?.Invoke();
    }
    #endregion


    //public void ShootDelayTimer()
    //{
    //    if (_shootDelayCoroutine == null)
    //        _shootDelayCoroutine = StartCoroutine(ShootDelayTimerRoutine());                    // Cache coroutine so we can can create a bool null check.
    //    else
    //        return;
    //}

    int RandomDamageDealt()     
    {
        int randomNumber = UnityEngine.Random.Range(_minDiceRoll, _maxDiceRoll);
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
    //IEnumerator ReloadingRoutine()
    //{
    //    _audioSource.clip = _weaponReloadClip;          // Assign reloading audio clip
    //    _audioSource.Play();
    //    yield return _weaponReloadClip.length;          // Yield until the clip finishes playing
    //    //AmmoManager.Instance.IsReloading = true;          
    //    AmmoManager.Instance.UpdateAmmoCount();         // Set Ammo Display count to 10
    //    _reloadingCoroutine = null;                     // Let the player be able to reload again
    //}
}
