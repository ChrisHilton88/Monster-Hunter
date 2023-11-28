using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : EnemyBase, IDamageable
{
    // 5 shot enemy
    // Anim States - Idle, Walk, Death, Shield

    public int _health;


    void OnEnable()
    {
        base.GrabComponents();
    }

    void Start()
    {
        //_health = _enemyHealth;
        _agent.destination = SpawnManager.Instance.EndPoint.position;
    }

    protected override void FixedUpdate()
    {
        if (_agent.velocity.magnitude > 0.01f)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
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

    protected override void CheckState()
    {
        throw new System.NotImplementedException();
    }
}
