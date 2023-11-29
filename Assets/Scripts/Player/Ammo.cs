using System;
using UnityEngine;

public class Ammo : MonoSingleton<Ammo>
{
    private bool _isReloading = false;
    public bool IsReloading { get; set; }

    private int _minAmmo = 0, _maxAmmo = 50, bullet = 1, _currentAmmoCount;
    public int MaxAmmo
    {
        get { return _maxAmmo; }
    }
    public int AmmoCount
    {
        get { return _currentAmmoCount; }
        set { _currentAmmoCount = value; }
    }
    public int CurrentAmmoCount { get { return _currentAmmoCount; } }   

    private bool _canShoot;
    public bool CanShoot
    {
        get { return _canShoot; }
        private set { _canShoot = value; }
    }


    #region Initialisation
    private void OnEnable()
    {
        WeaponShooting.reloadWeapon += UpdateInternalAmmoCountOnReload;
        WeaponShooting.shootWeapon += ShotBullet;
    }

    private void OnDisable()
    {
        WeaponShooting.reloadWeapon -= UpdateInternalAmmoCountOnReload;
        WeaponShooting.shootWeapon -= ShotBullet;
    }

    private void Start()
    {
        _currentAmmoCount = _maxAmmo;
        CanShoot = true;
    }
    #endregion

    #region Events
    // Responsible for updating the internal ammo count
    private void UpdateInternalAmmoCountOnReload()
    {
        _currentAmmoCount = _maxAmmo;
        Debug.Log("Updated internal ammo count to: " + _maxAmmo);
    }

    // Responsible for updating internally the current ammo count when shooting a bullet
    private void ShotBullet()
    {
        _currentAmmoCount -= bullet;
        Debug.Log("Current Ammo Count: " + _currentAmmoCount);
    }
    #endregion
}
