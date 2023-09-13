using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

//[RequireComponent (typeof(Animator), typeof(NavMeshAgent))]
public class Warlock : EnemyBase, IDamageable
{
    NavMeshAgent _agent;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.destination = SpawnManager.Instance.EndPoint.position;
    }

    void Update()
    {

    }

    protected override void Movement()
    {
        
    }

    protected override void Die()
    {
        base.Die();     // Calling the base Die() method unless we choose to add some here

        // Tell the Animator to Play the Die animation
        // Disable colliders to make sure that nothing can affect it
        // Disable game object when animation finishes
        // Return game object to the enemy list pool manager
    }

    public void ReceiveDamage(int damage)
    {
        
    }
}
