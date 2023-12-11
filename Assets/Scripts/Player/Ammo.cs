// Responsible for managing Player Ammo including Reloading
using System.Collections;
using UnityEngine;

public class Ammo : MonoSingleton<Ammo>
{
    private int _minAmmo = 0, _maxAmmo = 5;            // Sets the minimum and maximum Ammo Count
    private int _currentAmmoCount;

    private bool _canShoot;
    private bool _canReload;

    private WaitForSeconds _startGameShotDelayTimer = new WaitForSeconds(1f);

    #region Properties
    public int MinAmmo { get { return _minAmmo; } private set { _minAmmo = value; } }
    public int MaxAmmo { get { return _maxAmmo; } private set { _maxAmmo = value; } }   
    public int CurrentAmmoCount { get { return _currentAmmoCount; } private set { _currentAmmoCount = value; } }
    public bool CanReload { get { return _canReload; } set { _canReload = value; } }
    public bool CanShoot { get {  return _canShoot; } set { _canShoot = value; } }
    #endregion


    #region Initialisation
    private void OnEnable()
    {
        WeaponShooting.OnReloadWeapon += ReloadingWeapon;
        WeaponShooting.OnShootWeapon += ShotBullet;
    }

    private void OnDisable()
    {
        WeaponShooting.OnReloadWeapon -= ReloadingWeapon;
        WeaponShooting.OnShootWeapon -= ShotBullet;
    }

    private void Start()
    {
        CanReload = true;
        CurrentAmmoCount = MaxAmmo;             
        StartCoroutine(StartingGameShotDelayRoutine());
    }
    #endregion

    #region Events
    // Event only gets triggered if CanReload = true / check is on InputManager event
    public void ReloadingWeapon()
    {
        if (!CanReload)
        {
            Debug.Log("Reloading weapon");
            CurrentAmmoCount = MaxAmmo;
        }
    }

    // Event only gets triggered if CanShoot = true / check is on InputManager event
    public void ShotBullet()
    {
        CurrentAmmoCount--;

        if(CurrentAmmoCount <= 0)
        {
            CurrentAmmoCount = MinAmmo;
        }

        UIManager.Instance.ReduceBulletCount();
    }
    #endregion

    #region Coroutines
    private IEnumerator StartingGameShotDelayRoutine()
    {
        CanShoot = false;
        yield return _startGameShotDelayTimer;
        CanShoot = true;
    }
    #endregion
}
