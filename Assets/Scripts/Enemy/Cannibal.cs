public class Cannibal : EnemyBase, IDamageable
{
    // 2 shot enemy
    // Anim States - Idle, Walk, Run, Hide, Death, Cannibalise, Flinch

    private int _cannibalHealth = 150;
    private int _cannibalAgentSpeed = 6;
    private int _cannibalPointsUponDeath = 75;

    #region Properties
    protected int CannibalHealth { get { return _cannibalHealth; } set { _cannibalHealth = value; } }
    protected int CannibalAgentSpeed { get { return _cannibalAgentSpeed; } }
    protected int CannibalPointsUponDeath { get { return _cannibalPointsUponDeath; } }
    #endregion



    #region Initialisation
    // Values need to be re-assigned upon Enable
    protected sealed override void OnEnable()
    {
        EnemyHealth = CannibalHealth;
        AgentSpeed = CannibalAgentSpeed;
        base.OnEnable();
    }

    // Value is static, so only needs to be applied once
    protected void Start()
    {
        AgentPointsUponDeath = CannibalPointsUponDeath;
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
