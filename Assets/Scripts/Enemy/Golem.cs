public class Golem : EnemyBase, IDamageable
{
    // 4 shot enemy
    // Anim States - Idle, Walk, Charge, Death

    private int _golemHealth = 300;
    private int _golemAgentSpeed = 6;
    private int _golemPointsUponDeath = 300;

    #region Properties
    protected int Health { get { return _golemHealth; } set { _golemHealth = value; } }
    protected int GolemAgentSpeed { get { return _golemAgentSpeed; } }
    protected int GolemPointsUponDeath { get { return _golemPointsUponDeath; } }
    #endregion



    #region Initialisation
    // Values need to be re-assigned upon Enable
    protected sealed override void OnEnable()
    {
        EnemyHealth = Health;
        AgentSpeed = GolemAgentSpeed;
        base.OnEnable();
    }

    // Value is static, so only needs to be applied once
    protected void Start()
    {
        AgentPointsUponDeath = GolemPointsUponDeath;
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
