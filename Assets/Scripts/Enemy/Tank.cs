public class Tank : EnemyBase, IDamageable
{
    // 5 shot enemy
    // Anim States - Idle, Walk, Death, Shield

    private int _tankHealth = 100;
    private int _tankAgentSpeed = 3;
    private int _tankPointsUponDeath = 400;

    #region Properties
    protected int TankHealth { get { return _tankHealth; } set { _tankHealth = value; } }
    protected int TankAgentSpeed { get { return _tankAgentSpeed; } }
    protected int TankPointsUponDeath { get { return _tankPointsUponDeath; } }
    #endregion



    #region Initialisation
    // Values need to be re-assigned upon Enable
    protected sealed override void OnEnable()
    {
        EnemyHealth = TankHealth;
        AgentSpeed = TankAgentSpeed;
        base.OnEnable();
    }

    // Value is static, so only needs to be applied once
    protected void Start()
    {
        AgentPointsUponDeath = TankPointsUponDeath;
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
