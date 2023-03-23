using System.Collections;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    // Shoot raycast to middle of screen position (crosshair)
    // Build a raycast from the end of the weapon barrel (raycast origin)
    // Make the color red so we can see it
    // Limit it's distance, don't use infinity.

    Vector3 _reticulePos = new Vector3(0.5f, 0.5f, 0);

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

    public void ShootBullet()
    {
        GameObject newBullet = Instantiate(_bulletPrefab, _bulletSpawnPos.position, Quaternion.LookRotation(transform.forward));        // Sets rotation of bullet to forward spawning direction
        newBullet.transform.parent = _bulletContainer.transform;
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
        _audioSource.clip = _weaponReloadClip;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length);
        _shootDelayCoroutine = null;
        CanShoot = true;
    }




    //public void RaycastShoot()
    //{
    //    Ray rayOrigin = _mainCam.ViewportPointToRay(_reticulePos);
    //    RaycastHit hitInfo;

    //    if(Physics.Raycast(rayOrigin, out hitInfo))
    //    {
    //        Debug.Log("I found a: " + hitInfo.collider.tag);
    //    }
    //}


    // Fix - Draw bullet from an Object Pool instead of instantiating.

}
