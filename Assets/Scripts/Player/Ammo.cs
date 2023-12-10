// Responsible for managing Player Ammo
using System;
using System.Collections;
using UnityEngine;

public class Ammo : MonoSingleton<Ammo>
{
    private int _minAmmo = 0, _maxAmmo = 5;            // Sets the minimum and maximum Ammo Count
    private int _currentAmmoCount;

    private bool _canShoot;
    private bool _isReloading = false;

    [SerializeField] private AudioSource _audioSourceOnPlayer;

    private WaitForSeconds _startGameShotDelayTimer = new WaitForSeconds(2f);

    #region Properties
    public int MinAmmo { get { return _minAmmo; } private set { _minAmmo = value; } }
    public int MaxAmmo { get { return _maxAmmo; } private set { _maxAmmo = value; } }   
    public int CurrentAmmoCount { get { return _currentAmmoCount; } private set { _currentAmmoCount = value; } }
    public bool IsReloading { get { return _isReloading; } private set { _isReloading = value; } }
    public bool CanShoot { get {  return _canShoot; } private set { _canShoot = value; } }
    #endregion


    #region Initialisation
    private void OnEnable()
    {
        //WeaponShooting.reloadWeapon += UpdateInternalAmmoCountOnReload;
        WeaponShooting.OnShootWeapon += ShotBullet;
    }

    private void OnDisable()
    {
        //WeaponShooting.reloadWeapon -= UpdateInternalAmmoCountOnReload;
        WeaponShooting.OnShootWeapon -= ShotBullet;
    }

    private void Start()
    {
        CurrentAmmoCount = MaxAmmo;             
        // TODO: Make a short delay so the player can't shoot and waste ammo while loading
        StartCoroutine(StartingGameShotDelayRoutine());
    }
    #endregion

    #region Events
    //// Responsible for updating the internal ammo count to max ammo on a Reload
    //private void UpdateInternalAmmoCountOnReload()
    //{
    //    _currentAmmoCount = _maxAmmo;
    //}

    // Responsible for updating internally the current ammo count when shooting a bullet
    public void ShotBullet()
    {
        CurrentAmmoCount--;

        StartCoroutine(DelayTimeBetweenBulletsRoutine());   

        if(CurrentAmmoCount <= 0)
        {
            CurrentAmmoCount = MinAmmo;
            StartCoroutine(DelayTimeBetweenEmptyAmmoRoutine());
        }

        UIManager.Instance.ReduceBulletCount();
    }
    #endregion

    #region Coroutines
    private IEnumerator DelayTimeBetweenBulletsRoutine()
    {
        CanShoot = false;
        float timer = _audioSourceOnPlayer.clip.length;
        yield return new WaitForSeconds(timer);
        CanShoot = true;
    }

    private IEnumerator DelayTimeBetweenEmptyAmmoRoutine()
    {
        float timer = _audioSourceOnPlayer.clip.length;
        yield return new WaitForSeconds(timer);
    }

    private IEnumerator StartingGameShotDelayRoutine()
    {
        CanShoot = false;
        yield return _startGameShotDelayTimer;
        CanShoot = true;
    }
    #endregion
}
