using System.Collections;
using UnityEngine;

// Responsible for the environmental barrier game object behaviours
public class BarrierBehaviour : MonoBehaviour, IDamageable
{
    private int _maxHealth = 500;
    private int _destroyed = 0;

    private float _idleRepairTimer = 5f;         // When the automatic repair of a destroyable object should start
    private float _repairMultiplier = 10f;
    [SerializeField] private float _health = 500;

    private BoxCollider _boxCollider;
    private Coroutine _cooldownTimerRoutine;
    private Coroutine _destroyRoutine;
    private Coroutine _repairRoutine;
    private MeshRenderer _meshRenderer;
    

    [SerializeField] private ParticleSystem _repairParticleSystem;
    [SerializeField] private ParticleSystem _destroyedParticleSystem;
    [SerializeField] private GameObject _repairIcon;

    #region Properties
    public int MaxHealth { get { return _maxHealth; } }
    public float RepairMultiplier { get { return _repairMultiplier; } }
    public float Health { get { return _health; } set { _health = value; } }
    #endregion


    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>(); 
        _meshRenderer = GetComponent<MeshRenderer>();
        _cooldownTimerRoutine = null;
        _destroyRoutine = null;
        _repairRoutine = null;  
    }

    #region Interfaces
    // Responsible for taking damage and destroying object when Health < 0
    public void ReceiveDamage(int damageReceived)
    {
        Health -= damageReceived;
        if (_repairRoutine != null)
        {
            StopCoroutine(_repairRoutine);
            _repairParticleSystem.gameObject.SetActive(false);
            _repairIcon.SetActive(false);
            _repairRoutine = null;
        }

        if (Health < _destroyed)
        {
            ObjectDestroyed();
        }
        else
        {
            if(_cooldownTimerRoutine == null)
            {
                _cooldownTimerRoutine = StartCoroutine(CooldownTimerRoutine());
            }
            else
            {
                StopCoroutine(_cooldownTimerRoutine);
                _cooldownTimerRoutine = StartCoroutine(CooldownTimerRoutine());
            }
        }
    }
    #endregion

    #region Methods
    // Responsible for the behaviour of an object being destroyed
    private void ObjectDestroyed()
    {
        _destroyedParticleSystem.gameObject.SetActive(true);
        _boxCollider.enabled = false;
        _meshRenderer.enabled = false;
        StopCoroutine(_cooldownTimerRoutine);
        if (_destroyRoutine == null)
            _destroyRoutine = StartCoroutine(PlayDestroyedParticleSystemRoutine());
    }
    #endregion

    #region Coroutines
    // Responsible for playing the destroyed particle system and disabling when finished
    private IEnumerator PlayDestroyedParticleSystemRoutine()
    {
        while (_destroyedParticleSystem.isPlaying)
        {
            yield return null;
        }
        
        _destroyedParticleSystem.gameObject.SetActive(false);
        gameObject.SetActive(false);    
        _destroyRoutine = null;
    }

    private IEnumerator CooldownTimerRoutine()
    {
        float timer = 0;

        while (timer <= _idleRepairTimer)
        {
            timer += Time.deltaTime;
            Debug.Log("Timer: " + timer);
            yield return null;
        }

        if (_repairRoutine == null)
            _repairRoutine = StartCoroutine(PlayRepairParticleSystemRoutine());

        _cooldownTimerRoutine = null;
    }

    // Responsible for playing the repair particle system and disabling when object gets hit again or reach max health
    private IEnumerator PlayRepairParticleSystemRoutine()
    {
        _repairParticleSystem.gameObject.SetActive(true);
        _repairIcon.SetActive(true);

        while(Health < MaxHealth)
        {
            float increment = RepairMultiplier * Time.deltaTime;
            Health += increment;
            Debug.Log("Health: " + Health);
            yield return null;
        }

        _repairParticleSystem.gameObject.SetActive(false);
        _repairIcon.SetActive(false);
        Health = MaxHealth;
        _repairRoutine = null;
    }
    #endregion
}
