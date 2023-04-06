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

    private float _maxShootDistace = 500f;

    Coroutine _shootDelayCoroutine;
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
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Debug.DrawRay(_bulletSpawnPos.position, _mainCam.ViewportPointToRay(_reticulePos).direction * _maxShootDistace, Color.red);     // Draws a line from gun to crosshair
    }

    // Request a bullet from ObjectPoolManager and Shoot.
    public void ShootBullet()
    {
        _bulletRotation.SetLookRotation(transform.forward);             // Sets bullet rotation to face the forward direction of the gun.
        ObjectPoolManager.Instance.RequestBullet(_bulletSpawnPos, _bulletRotation);         // Move bullet objects to gun and passes in rotation.
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
        yield return new WaitForSeconds(_audioSource.clip.length);                  // Can't make changes unless using a separate AudioSource component, OR amend audio clip.
        //_audioSource.clip = _weaponReloadClip;
        //_audioSource.Play();
        //yield return new WaitForSeconds(_audioSource.clip.length);
        _shootDelayCoroutine = null;
        CanShoot = true;
    }
}
