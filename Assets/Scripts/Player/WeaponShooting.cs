using System;
using System.Collections;
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

    private Coroutine _emptyAmmoCoroutine;
    private Coroutine _shootDelayCoroutine;
    private Coroutine _reloadingCoroutine;

    public static Action OnEmptyClip;        // Event that is responsible for managing audio/UI when Ammo clip is empty  
    public static Action OnReloadWeapon;     // Event that is responsible for reloading ammo in current weapon
    public static Action OnShootWeapon;      // Event that is responsible for passing ammo when shooting a weapon 



    #region Initialisation
    void OnEnable()
    {
        OnEmptyClip += EmptyAmmoClipEvent;
        OnReloadWeapon += ReloadWeaponEvent;
        OnShootWeapon += ShootBulletEvent;
    }

    void OnDisable()
    {
        OnEmptyClip -= EmptyAmmoClipEvent;
        OnReloadWeapon -= ReloadWeaponEvent;
        OnShootWeapon -= ShootBulletEvent;
    }

    void Start()
    {
        _emptyAmmoCoroutine = null; 
        _shootDelayCoroutine = null;
        _reloadingCoroutine = null;
        _audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Methods
    private int RandomDamageDealt()
    {
        int randomNumber = UnityEngine.Random.Range(_minDiceRoll, _maxDiceRoll);
        return randomNumber;
    }
    #endregion

    #region Events
    private void EmptyAmmoClipEvent()
    {
        if(_emptyAmmoCoroutine == null)
            _emptyAmmoCoroutine = StartCoroutine(EmptyAmmoDelayTimerRoutine());
    }

    private void ReloadWeaponEvent()
    {
        if (_reloadingCoroutine == null)
            _reloadingCoroutine = StartCoroutine(ReloadWeaponDelayTimerRoutine());   
    }

    private void ShootBulletEvent()
    {
        if (Ammo.Instance.CurrentAmmoCount > 0)               // Check that there is a bullet available
        {
            if(_shootDelayCoroutine == null)
                _shootDelayCoroutine = StartCoroutine(ShootDelayTimerRoutine());

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
                    damageable.ReceiveDamage(damageDealt);      // Passes in damage to the enemy hit
                }
                else
                    Debug.Log("Hit target IDamageable is NULL - WeaponShooting class");
            }
        }
        else
            OnEmptyClip?.Invoke();          // Ammo must be empty
    }
    #endregion

    #region Coroutines
    private IEnumerator EmptyAmmoDelayTimerRoutine()
    {
        _audioSource.clip = _emptyAmmoClip;
        _audioSource.volume = 0.5f;
        _audioSource.Play();
        yield return new WaitForSeconds(_emptyAmmoClip.length);
        _emptyAmmoCoroutine = null;
    }

    private IEnumerator ShootDelayTimerRoutine()
    {
        Ammo.Instance.CanShoot = false;
        _audioSource.clip = _weaponFiredClip;
        _audioSource.volume = 0.5f;
        _audioSource.Play();
        yield return new WaitForSeconds(_weaponFiredClip.length);     
        Ammo.Instance.CanShoot = true;
        _shootDelayCoroutine = null;
    }

    private IEnumerator ReloadWeaponDelayTimerRoutine()
    {
        Ammo.Instance.CanShoot = false;
        Ammo.Instance.CanReload = false;
        _audioSource.clip = _weaponReloadClip;          
        _audioSource.volume = 0.5f;
        _audioSource.Play();
        yield return new WaitForSeconds(_weaponReloadClip.length);
        Ammo.Instance.CanShoot = true;
        Ammo.Instance.CanReload = true;      
        _reloadingCoroutine = null;                     
        UIManager.Instance.UpdateAmmoDisplayOnReload();
    }

    // Responsible for handling timing events for shooting, reloading and empty clip
    private IEnumerator WeaponBehaviourDelayTimerRoutine(bool test, AudioClip clip, float _clipVolume, float _clipLength, Coroutine co)
    {
        _audioSource.clip = clip;
        _audioSource.volume = _clipVolume;
        _audioSource.Play();
        yield return new WaitForSeconds(_clipLength);
        co = null;
    }
    #endregion
}
