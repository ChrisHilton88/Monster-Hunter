using System.Collections;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    private readonly Vector3 _reticulePos = new Vector3(0.5f, 0.5f, 0);
    private Quaternion _bulletRotation;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _weaponFiredClip;
    [SerializeField] private AudioClip _weaponReloadClip;

    [SerializeField] private Transform _laserOrigin;
    [SerializeField] private Transform _crosshair;
    [SerializeField] private Transform _bulletSpawnPos;
    [SerializeField] private Camera _mainCam;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletContainer;

    private int _reloadAmount = 10;

    private float _maxShootDistace = 500f;

    Coroutine _shootDelayCoroutine;
    Coroutine _reloadingCoroutine;
    WaitForSeconds _shootDelayTime = new WaitForSeconds(2f);

    private bool _canShoot;
    public bool CanShoot
    {
        get { return _canShoot; }
        private set { _canShoot = value; }  
    }


    void Start()
    {
        _shootDelayCoroutine = null;
        _reloadingCoroutine = null;
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Debug.DrawRay(_bulletSpawnPos.position, _mainCam.ViewportPointToRay(_reticulePos).direction * _maxShootDistace, Color.red);     // Draws a line from gun to crosshair
    }

    // Request a bullet from ObjectPoolManager and Shoot.
    public void ShootBullet()
    {
        if(UIManager.Instance.AmmoCount > 0)
        {
            _bulletRotation.SetLookRotation(transform.forward);             // Sets bullet rotation to face the forward direction of the gun.
            ObjectPoolManager.Instance.RequestBullet(_bulletSpawnPos, _bulletRotation);         // Move bullet objects to gun and passes in rotation.
            UIManager.Instance.UpdateAmmoCount(1);
        }
        else
        {
            Debug.Log("Testing");
            // Add a display message and audio sound to tell the player they need to reload.
            // Play audio sounds for reloading, and once reloaded set the count to 10.
            _reloadingCoroutine = StartCoroutine(ReloadingRoutine());
        }
    }

    public void ShootDelayTimer()
    {
        if (_shootDelayCoroutine == null)
            _shootDelayCoroutine = StartCoroutine(ShootDelayTimerRoutine());                    // Cache coroutine so we can can create a bool null check.
        else
            return;
    }

    // Coroutine controlling delay time of shooting and audio 
    IEnumerator ShootDelayTimerRoutine()
    {
        CanShoot = false;
        _audioSource.clip = _weaponFiredClip;
         _audioSource.Play();
        yield return new WaitForSeconds(_weaponFiredClip.length);                  // Can't make changes unless using a separate AudioSource component, OR amend audio clip.
        Debug.Log(_weaponReloadClip.length.ToString());    
        _shootDelayCoroutine = null;
        CanShoot = true;
    }

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
