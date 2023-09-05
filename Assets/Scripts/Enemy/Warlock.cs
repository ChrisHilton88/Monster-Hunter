using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Animator), typeof(NavMeshAgent))]
public class Warlock : EnemyBase, IDamageable
{


    void Start()
    {
        Debug.Log(_enemyHealth);
    }

    void Update()
    {
        Movement();
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
