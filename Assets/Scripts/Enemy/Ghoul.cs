public class Ghoul : EnemyBase, IDamageable
{
    // 1 shot enemy
    // Anim States - Idle, Walk, Run, Death, Frenzy

    private int _ghoulHealth = 100;
    private int _ghoulAgentSpeed = 4;
    private int _ghoulPointsUponDeath = 50;

    #region Properties
    protected int GhoulHealth { get { return _ghoulHealth; } set { _ghoulHealth = value; } }
    protected int GhoulAgentSpeed { get { return _ghoulAgentSpeed; } }
    protected int GhoulPointsUponDeath { get { return _ghoulPointsUponDeath; } }
    #endregion



    #region Initialisation
    // Values need to be re-assigned upon Enable
    protected sealed override void OnEnable()
    {
        AgentSpeed = GhoulAgentSpeed;
        EnemyHealth = GhoulHealth;
        base.OnEnable();
    }

    // Value is static, so only needs to be applied once
    protected void Start()
    {
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
