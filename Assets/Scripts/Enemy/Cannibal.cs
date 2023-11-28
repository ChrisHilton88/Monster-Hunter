using UnityEngine;
using UnityEngine.AI;

public class Cannibal : EnemyBase, IDamageable
{
    // 2 shot enemy
    // Anim States - Idle, Walk, Run, Hide, Death, Cannibalise, Flinch

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

    protected override void CheckState()
    {
        throw new System.NotImplementedException();
    }
}
