using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

// This will be the base class for all enemies and is inherited

public abstract class EnemyBase : MonoBehaviour
{
    [Range(100, 501)] public int _enemyHealth;     
    [Range(5, 50)] [SerializeField] public float _enemyMovementSpeed;

    private Vector3 _spawnPoint = new Vector3(0, 0, 0);     // Update to actual start point when finalised
    private Vector3 _endPoint = new Vector3(-24, 13.5f, 1);
    private Quaternion _spawnRotation = Quaternion.identity;        // Make sure spawn rotation is always facing the door

    public Animator _animator;
    public NavMeshAgent _agent;

    // Create a Controller to assign
    [SerializeField] private AnimatorController _animController;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Movement()
    {
        // Set the end destination for all enemies to be the same point
        // How do we get a reference to the end point position? 
            // Can't create a SerializeField to drag and drop a Transform position onto
        _agent.destination = _endPoint;
    }
    

    protected virtual void Die()
    {
        // Play death animation...
        // Add different death animations or VFX such as disintegrate
    }

    // VIRTUAL VS ABSTRACT METHODS:
    // The ABSTRACT method is a method that is declared in an abstract class but is not implemented (CANNOT declare a body). It is an INCOMPLETE method
    // The VIRTUAL method is declared in a base class and has an implementation, but the child class may override the default implementation
}
