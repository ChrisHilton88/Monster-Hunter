using System.Collections;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float _speed = 100f;

    private readonly Vector3 _bulletScale = new Vector3(200, 200, 200);

    Rigidbody _rb;
    AudioSource _audioSource;
    AudioClip _currentAudioClip;
    Coroutine _audioClipRoutine;

    [SerializeField] private AudioClip _bulletMetalHitClip;


    void Start()
    {
        transform.localScale = _bulletScale;
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Not shooting in the direction the laser is pointing
        _rb.velocity = transform.TransformDirection(Vector3.forward * _speed); 

        //Debug.DrawRay(_bulletSpawnPos.position, _mainCam.ViewportPointToRay(_reticulePos).direction);     

        // Add more here as bullet is not bouncing off surfaces and instead fights against the collider until it is able to continue moving forward.
    }

    void OnCollisionEnter(Collision other)          // Collide with another collider
    {
        if(_audioClipRoutine == null)               // Check if the coroutine is already running, if so, do nothing.
        {
            SwitchThroughTags(other);               // Switch through the different game object tags
            _audioClipRoutine = StartCoroutine(PlayAudioClipOnce());            // Assign coroutine to be treated as a bool so it only plays once.
        }
    }

    // Runs a switch statement through game object TAGS
    void SwitchThroughTags(Collision other)
    {
        switch (other.collider.tag)
        {
            case StringManager._wallTag:                        // Fall through cases. Wall, Floor & Console will fall through to Column because it runs the same code.
            case StringManager._floorTag:
            case StringManager._consoleTag:
            case StringManager._columnTag:
                _audioSource.clip = _bulletMetalHitClip;        // Assign audio clip
                _currentAudioClip = _bulletMetalHitClip;
                break;
            case StringManager._enemy:
                Debug.Log("About time you hit something worthwhile");
                break;
            default:
                break;
        }

        Debug.Log("The Bullet hit: " + other.collider.tag);
    }

    // It as bool routine for making sure only 1 audio sound effect is played.
    IEnumerator PlayAudioClipOnce()
    {
        _audioSource.Play();                                    // Play the audioclip that was assigned in switch statement.
        yield return new WaitForSeconds(_currentAudioClip.length);
        gameObject.SetActive(false);
        _audioClipRoutine = null;
    }
}
