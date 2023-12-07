using UnityEngine;

public class Ghoul : EnemyBase, IDamageable
{
    // 1 shot enemy
    // Anim States - Idle, Walk, Run, Death, Frenzy

    private int _health = 100;
    private int _ghoulAgentSpeed = 4;
    private int _ghoulPointsUponDeath = 50;

    #region Properties
    protected int Health { get { return _health; } set { _health = value; } }
    protected int GhoulAgentSpeed { get { return _ghoulAgentSpeed; } }
    protected int GhoulPointsUponDeath { get { return _ghoulPointsUponDeath; } }
    #endregion



    #region Initialisation
    protected sealed override void OnEnable()
    {
        base.OnEnable();
        EnemyHealth = Health;       // Keep setting EnemyHealth back to 100 when re-enabling game object
        AgentSpeed = GhoulAgentSpeed;
    }

    protected sealed override void Start()
    {
        base.Initialisation();
        AgentPointsUponDeath = GhoulPointsUponDeath;
    }
    #endregion

    #region Methods
    protected override sealed void FixedUpdate()
    {
        base.CheckState();

        if (_agent.velocity.magnitude > 0.01f)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    protected sealed override void CheckState()
    {
        // Add personalised state and behaviour
    }
    #endregion

    // Interface
    public void ReceiveDamage(int damageReceived)
    {
        if (damageReceived > EnemyHealth)
        {
            EnemyHealth = 0;
            base.Die();
        }
        else
        {
            EnemyHealth -= damageReceived;
        }
    }
}
