using System;
using UnityEngine;

public class AmmoManager : MonoSingleton<AmmoManager>
{
    private bool _isReloading = false;
    public bool IsReloading { get; set; }

    private int _minAmmo = 0, _maxAmmo = 50, _currentAmmoCount;

    public int MaxAmmo
    {
        get { return _maxAmmo; }
    }
        
    public int AmmoCount
    {
        get { return _currentAmmoCount; }
        set { _currentAmmoCount = value; }
    }


    void OnEnable()
    {
        InputManager.reloadWeapon += UpdateAmmoCount;
    }

    void Start()
    {
        _currentAmmoCount = _maxAmmo;
    }

    public void UpdateAmmoCount()
    {
        _currentAmmoCount = _maxAmmo;
        Debug.Log("Updated internal ammo count to: " + _maxAmmo);
    }

    void OnDisable()
    {
        InputManager.reloadWeapon -= UpdateAmmoCount;   
    }
}
