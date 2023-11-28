using UnityEngine;

public class Ghoul : EnemyBase, IDamageable
{
    // 1 shot enemy
    // Anim States - Idle, Walk, Run, Death, Frenzy

    private int _health = 100;


    #region Properties
    protected int Health { get { return _health; } set { _health = value; } }
    #endregion


    protected override sealed void Start()
    {
        EnemyHealth = Health;
    }

    protected override sealed void FixedUpdate()
    {
        base.CheckState();

        //if (_agent.velocity.magnitude > 0.01f)
        //{
        //    _animator.SetBool("IsMoving", true);
        //}
        //else
        //{
        //    _animator.SetBool("IsMoving", false);
        //}
    }

    protected sealed override void CheckState()
    {
        
    }

    public void ReceiveDamage(int damageReceived)
    {
        if (damageReceived > _health)
        {
            _health = 0;
            base.Die();
        }
        else
        {
            Debug.Log("Hit: " + gameObject.name);
            _health -= damageReceived;
        }
    }

    public void ShowDamage()
    {

    }
}
