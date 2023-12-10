// Responsible for managing Player Ammo
using System;

public class Ammo : MonoSingleton<Ammo>
{
    private int _minAmmo = 0, _maxAmmo = 5;            // Sets the minimum and maximum Ammo Count
    private int _currentAmmoCount;

    private bool _canShoot;
    private bool _isReloading = false;

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
        CurrentAmmoCount = MaxAmmo;             // At start of game, set the Ammo to the maximum amount
        // TODO: Make a short delay so the player can't shoot and waste ammo while loading
        CanShoot = true;                        // Make sure Player can shoot at the start of the game
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

        if(CurrentAmmoCount <= 0)
        {
            CurrentAmmoCount = MinAmmo;
        }

        UIManager.Instance.ReduceBulletCount();
    }
    #endregion
}
