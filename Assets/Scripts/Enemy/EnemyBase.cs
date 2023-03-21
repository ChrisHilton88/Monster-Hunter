using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;


// This will be the base class for all enemies

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    [Range(100, 501)] [SerializeField] private int health;
    [Range(5, 50)] [SerializeField] private float _movementSpeed;

    private Vector3 _spawnPoint = new Vector3(0, 0, 0);
    private Quaternion _spawnRotation = Quaternion.identity;

    private Animator _anim;
    private NavMeshAgent _agent;

    // Create a Controller to assign.
    [SerializeField] private AnimatorController _animController;



    void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        // Assign all the fields in the required components.
    }

    // Make virtual so it can be overridden.
    protected virtual void Movement()
    {
        // Add individual movement code
    }

    void Die()
    {
        this.gameObject.SetActive(false);   
    }
}
