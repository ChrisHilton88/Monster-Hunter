using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Animator), typeof(NavMeshAgent))]
public class Warlock : EnemyBase, IDamageable
{
    NavMeshAgent _agent;
    Animator _animator;     // Anim states - Idle, Walk, Hide, Death, Scream, Teleport, Summon



    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.destination = SpawnManager.Instance.EndPoint.position;     // Set agents initial end point
    }

    void Update()
    {
        // Set these up as events so we don't need to be constantly checking and setting
        if(_agent.velocity.magnitude > 0.01f) 
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    protected override void Die()
    {
        _animator.SetTrigger("IsDead");

        //base.Die();     // Calling the base Die() method unless we choose to add some here

        // Tell the Animator to Play the Die animation
        // Disable colliders to make sure that nothing can affect it
        // Disable game object when animation finishes
        // Return game object to the enemy list pool manager
    }

    public void ReceiveDamage(int damage)
    {
        
    }


}
