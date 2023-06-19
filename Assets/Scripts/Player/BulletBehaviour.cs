using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(BulletBehaviour), (typeof(AudioSource)))]
public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private AudioClip _bulletMetalHitClip;

    AudioSource _audioSource;
    Coroutine _audioClipRoutine;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioClipRoutine = null;
    }

    void OnCollisionEnter(Collision other)          
    {
        // Disable game object
        gameObject.SetActive(false);

        if(_audioClipRoutine == null)               
        {
            SwitchThroughTags(other);                                           
            _audioClipRoutine = StartCoroutine(PlayAudioClipOnce());            
        }
    }

    // Switch statement using game object TAGS
    void SwitchThroughTags(Collision other)
    {
        switch (other.collider.tag)
        {
            case StringManager._wallTag:                        // Fall through cases. Wall, Floor & Console will fall through to Column.
            case StringManager._floorTag:
            case StringManager._consoleTag:
            case StringManager._columnTag:
                _audioSource.clip = _bulletMetalHitClip;        // Assign audio clip
                break;
            case StringManager._enemy:
                Debug.Log("About time you hit something worthwhile");
                break;
            default:
                break;
        }
    }

    IEnumerator PlayAudioClipOnce()
    {
        _audioSource.Play();                                    
        yield return new WaitForSeconds(_audioSource.clip.length);
        gameObject.SetActive(false);         
        _audioClipRoutine = null;
    }

    // Steps to create Ricochet bouncing effect:
    // 1. Make sure both game objects have a collider and at least 1 game object has a Rigidbody - Done
    // 2. Make sure bullet has force or initial velocity applied to it - Done
    // 3. Physics Materials - Create and assign Physics materials to both game objects. Adjust bounciness to be greater than 0 - Done
    // 4. Enable collision detection - Check collision matrix - Done
    // 5. Handle Collision Events - OnCollisionEnter - Done
    // 6. Reflect the bullets direction - Calculate the reflection direction of the bullet based on the collision normal - Done
}
