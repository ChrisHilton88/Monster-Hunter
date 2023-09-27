using System;
using UnityEngine;

public class AmmoManager : MonoSingleton<AmmoManager>
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



    void OnEnable()
    {
        WeaponShooting.reloadWeapon += UpdateAmmoCount;
        WeaponShooting.shootWeapon += ShotBullet;
    }

    void Start()
    {
        _currentAmmoCount = _maxAmmo;
        CanShoot = true;
    }

    void UpdateAmmoCount()
    {
        // Maybe some IF checks to see if it is a reload or a Shoot

        _currentAmmoCount = _maxAmmo;
        Debug.Log("Updated internal ammo count to: " + _maxAmmo);
    }

    void ShotBullet()
    {
        _currentAmmoCount -= bullet;
        Debug.Log("Current Ammo Count: " + _currentAmmoCount);
    }

    void OnDisable()
    {
        WeaponShooting.reloadWeapon -= UpdateAmmoCount;   
        WeaponShooting.shootWeapon -= ShotBullet;   
    }
}
