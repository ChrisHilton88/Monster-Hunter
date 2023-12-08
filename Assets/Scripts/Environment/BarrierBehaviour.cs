using System.Collections;
using UnityEngine;

// Responsible for the environmental barrier game object behaviours
public class BarrierBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 500;
    private int _destroyed = 0;

    private BoxCollider _boxCollider;
    private MeshRenderer _meshRenderer;

    [SerializeField] private ParticleSystem _particleSystem;


    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>(); 
        _meshRenderer = GetComponent<MeshRenderer>();   
    }

    #region Interfaces
    public void ReceiveDamage(int damageReceived)
    {
        _health -= damageReceived;

        if (_health < _destroyed)
        {
            _particleSystem.gameObject.SetActive(true);
            _boxCollider.enabled = false;
            _meshRenderer.enabled = false;
            StartCoroutine(WaitForDestroyedParticleSystemRoutine());
        }
    }
    #endregion

    #region Methods
    private void CooldownTimer()
    {
        // Keep setting timer back to 0 everytime the object gets hit
        // When the object hasn't been hit for 5 secs
        // Start repairing the object
        // Play particle effect/animation to show it is recharging
        // Stop playing effects when Health is back to full

        // Restart timer all over again if the object gets shot while repairing itself
    }
    #endregion

    #region Coroutines
    private IEnumerator WaitForDestroyedParticleSystemRoutine()
    {
        while (_particleSystem.isPlaying)
        {
            yield return null;
        }

        _particleSystem.gameObject.SetActive(false);
    }


    #endregion

}
