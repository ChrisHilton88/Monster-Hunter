public class Warlock : EnemyBase, IDamageable
{
    // 3 shot enemy
    // Anim states - Idle, Walk, Hide, Death, Scream, Teleport, Summon, Flinch

    // Take Damage Event - Display a UI text that shows the amount of damage dealt above the enemy
    // Update enemies health
    // Play sound effect
    // Play Flinch animation?
    // Check health is < 0 = Death animation
    // Add/Remove from total enemy count
    // Return back to Object Pool

    private int _warlockHealth = 250;
    private int _warlockAgentSpeed = 5;
    private int _warlockPointsUponDeath = 250;

    #region Properties
    protected int WarlockHealth { get { return _warlockHealth; } set { _warlockHealth = value; } }
    protected int WarlockAgentSpeed { get { return _warlockAgentSpeed; } }
    protected int WarlockPointsUponDeath { get { return _warlockPointsUponDeath; } }
    #endregion



    #region Initialisation
    // Values need to be re-assigned upon Enable
    protected sealed override void OnEnable()
    {
        EnemyHealth = WarlockHealth;
        AgentSpeed = WarlockAgentSpeed;
        base.OnEnable();
    }

    // Value is static, so only needs to be applied once
    protected void Start()
    {
        AgentPointsUponDeath = WarlockPointsUponDeath;
    }
    #endregion

    #region Methods
    protected override void FixedUpdate()
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

    protected override void CheckState()
    {
        // Add personalised state and behaviour
    }
    #endregion

    #region Interfaces
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
    #endregion
}
