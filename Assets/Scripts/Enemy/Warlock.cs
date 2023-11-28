using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Animator), typeof(NavMeshAgent))]
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
