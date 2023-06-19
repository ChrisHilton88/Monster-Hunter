using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooting : MonoBehaviour
{
    private readonly Vector3 _reticulePos = new Vector3(0.5f, 0.5f, 0);

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _weaponFiredClip;
    [SerializeField] private AudioClip _weaponReloadClip;

    [SerializeField] private Transform _laserOrigin;
    [SerializeField] private Transform _crosshair;              // Not used

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
        Debug.DrawRay(transform.position, Vector3.forward, Color.red);
    }

    // Called from InputManager
    // Request a bullet from ObjectPoolManager and Shoot.
    public void ShootBullet()
    {
        Debug.Log("Test 1");

        if (UIManager.Instance.AmmoCount > 0)               // Check that there is a bullet available
        {
            Debug.Log("Test 2");
            Ray rayOrigin = Camera.main.ViewportPointToRay(_reticulePos);
            RaycastHit hitInfo;
            Debug.Log("Shot Ray");



            // If the raycast hits a game object with an "Environment" layer on it
            if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Environment")))            // Layermask 6 is Environment
            {
                Debug.Log("Test 3");
                GameObject newObject = ObjectPoolManager.Instance.RequestBullet(hitInfo);
                UIManager.Instance.UpdateAmmoCount(1);
            }
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
        _shootDelayCoroutine = null;
        CanShoot = true;
    }

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
