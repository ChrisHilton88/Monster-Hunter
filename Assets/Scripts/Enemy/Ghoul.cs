using UnityEngine;

public class Ghoul : EnemyBase, IDamageable
{
    // 1 shot enemy
    // Anim States - Idle, Walk, Run, Death, Frenzy

    private int _health = 100;


    #region Properties
    protected int Health { get { return _health; } set { _health = value; } }
    #endregion


    protected sealed override void OnEnable()
    {
        base.OnEnable();
        EnemyHealth = Health;       // Keep setting EnemyHealth back to 100 when re-enabling game object
    }

    protected sealed override void Start()
    {
        
    }

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
        
    }

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
