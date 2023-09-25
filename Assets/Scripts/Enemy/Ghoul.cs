using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : EnemyBase, IDamageable
{
    // 1 shot enemy
    // Anim States - Idle, Walk, Run, Death, Frenzy

    public int _health;

    
    void OnEnable()
    {
        base.GrabComponents();
    }

    void Start()
    {
        _health = _enemyHealth;
        //_agent.destination = SpawnManager.Instance.EndPoint.position;
    }

    void Update()
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
}
