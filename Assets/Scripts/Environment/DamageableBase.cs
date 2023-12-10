using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
public abstract class DamageableBase : MonoBehaviour, IDamageable
{
    private int _maxHealth;             // Let the inheriting object determine it's Max Health
    private const int _destroyed = 0;   // Const - Consistent across all types/instances at compile time

    [SerializeField] private float _health;              // Variable for tracking game objects health
    private float _idleRepairTimer;     // Let inherting object determine how much wait time there is before starting to repair
    private float _repairMultiplier;    // Let inheriting object determine how fast/slow the object should repair

    protected Coroutine _cooldownTimerRoutine;      // Internal bool timer to start repair on game object
    protected Coroutine _destroyRoutine;            // Internal bool for destroying the game object
    protected Coroutine _repairRoutine;             // Internal bool for repairing the game object

    protected BoxCollider _boxCollider;
    protected MeshRenderer _meshRenderer;

    [SerializeField] protected ParticleSystem _repairParticleSystem;        // VFX - Healing 
    [SerializeField] protected ParticleSystem _destroyedParticleSystem;     // VFX - Smoke
    [SerializeField] protected GameObject _repairIcon;                      // Repair Icon that rotates above game object

    #region Properties
    protected int MaxHealth { get {  return _maxHealth; } set { _maxHealth = value; } }
    protected int Destroyed { get { return _destroyed; } }
    protected float Health { get { return _health; } set { _health = value; } } 
    protected float IdleRepairTimer { get { return _idleRepairTimer; } set { _idleRepairTimer = value; } }  
    protected float RepairMultiplier { get { return _repairMultiplier; } set { _repairMultiplier = value; } }   
    #endregion



    #region Initialisation
    protected virtual void Start()
    {
        GrabComponents();
        SetCoroutinesToNull();  
    }

    protected virtual void GrabComponents()                         // Grab the components from the game object
    {
        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();   
    }

    protected virtual void SetCoroutinesToNull()                    // Set all Coroutine variables to null 
    {
        _cooldownTimerRoutine = null;
        _destroyRoutine = null;
        _repairRoutine = null;
    }

    protected abstract void Initialisation();                       // Let inheriting class set initialisation values
    #endregion

    #region Methods
    protected virtual void DestroyThisObject()                      // Only called when an objects Health is below 0
    {
        _destroyedParticleSystem.gameObject.SetActive(true);        // Play destroyed smoke VFX
        _boxCollider.enabled = false;                               // Turn off BoxCollider component - So we can't continue to shoot/hit it
        _meshRenderer.enabled = false;                              // Turn off MeshRenderer component - So we can't see the object anymore
        // TODO: Check if this matters
        StopCoroutine(_cooldownTimerRoutine);                       // Stop running the Cooldown timer routine, otherwise it will play the other repair
        if (_destroyRoutine == null)                                // Check if we aren't already destroying ourselves
            _destroyRoutine = StartCoroutine(DestroyedObjectRoutine());
    }
    #endregion

    #region Interfaces
    // Responsible for taking damage and destroying object when Health < 0
    public void ReceiveDamage(int damageReceived)
    {
        Health -= damageReceived;

        // If the repair routine is already running, we need to stop it and wait for the timer again
        if (_repairRoutine != null)
        {
            StopCoroutine(_repairRoutine);
            _repairParticleSystem.gameObject.SetActive(false);
            _repairIcon.SetActive(false);
            _repairRoutine = null;
        }

        // Check if Health is below 0
        if (Health < _destroyed)
        {
            DestroyThisObject();
        }
        else
        {
            if (_cooldownTimerRoutine == null)
            {
                _cooldownTimerRoutine = StartCoroutine(CooldownTimerRoutine());     // Start IdleRepairTimer
            }
            else
            {
                StopCoroutine(_cooldownTimerRoutine);                               // Stop the IdleRepairCoroutine and start it again (below)
                _cooldownTimerRoutine = StartCoroutine(CooldownTimerRoutine());     // Start IdleRepairTimer again
            }
        }
    }
    #endregion

    #region Coroutines
    // Responsible for managing the timer of the game object and when it should start self-repairing
    private IEnumerator CooldownTimerRoutine()
    {
        float timer = 0;

        while (timer <= IdleRepairTimer)        // Don't do anything until this timer expires
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (_repairRoutine == null)
            _repairRoutine = StartCoroutine(RepairVFXRoutine());

        _cooldownTimerRoutine = null;
    }

    // Responsible for playing the destroyed particle system and disabling when finished
    private IEnumerator DestroyedObjectRoutine()
    {
        while (_destroyedParticleSystem.isPlaying)                  // While the VFX is playing, don't do anything
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }

    // Responsible for playing the repair particle system and disabling when object gets hit again or reach max health
    private IEnumerator RepairVFXRoutine()
    {
        _repairParticleSystem.gameObject.SetActive(true);           // Turn on VFX
        _repairIcon.SetActive(true);                                // Turn on Repair Icon

        while (Health < MaxHealth)                                  // While the game objects health is < MaxHealth, keep incrementing health and playing VFX
        {
            float increment = RepairMultiplier * Time.deltaTime;
            Health += increment;
            yield return null;
        }

        _repairParticleSystem.gameObject.SetActive(false);      // Turn off VFX
        _repairIcon.SetActive(false);                           // Turn off Repair Icon
        Health = MaxHealth;
        _repairRoutine = null;
    }
    #endregion
}
