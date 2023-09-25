using UnityEngine;
using UnityEngine.AI;

// This will be the base class for all enemies and is inherited

public abstract class EnemyBase : MonoBehaviour
{
    public int _enemyHealth = 100;

    public NavMeshAgent _agent;
    public Animator _animator;


    protected virtual void GrabComponents()
    {
        _agent = GetComponent<NavMeshAgent>();  
        _animator = GetComponent<Animator>();
    }

    protected virtual void Die()
    {
        _animator.SetTrigger("IsDead");
    }

    // VIRTUAL VS ABSTRACT METHODS:
    // The ABSTRACT method is a method that is declared in an abstract class but is not implemented (CANNOT declare a body). It is an INCOMPLETE method
    // The VIRTUAL method is declared in a base class and has an implementation, but the child class may override the default implementation
}
