using UnityEngine;
using UnityEngine.AI;

// This will be the base class for all enemies and is inherited

public abstract class EnemyBase : MonoBehaviour
{
    [Range(100, 501)] public int _enemyHealth;     

    // Don't need enemy movement speed variable - Can use NavMeshAgent speed field

    

    protected virtual void Die()
    {
        // Play death animation...
        // Add different death animations or VFX such as disintegrate
    }

    // VIRTUAL VS ABSTRACT METHODS:
    // The ABSTRACT method is a method that is declared in an abstract class but is not implemented (CANNOT declare a body). It is an INCOMPLETE method
    // The VIRTUAL method is declared in a base class and has an implementation, but the child class may override the default implementation
}
