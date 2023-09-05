using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;


// This will be the base class for all enemies and is inherited

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    [Range(100, 501)] [SerializeField] private int _enemyHealth;
    [Range(5, 50)] [SerializeField] private float _enemyMovementSpeed;

    private Vector3 _spawnPoint = new Vector3(0, 0, 0);     // Update to actual start point when finalised
    private Quaternion _spawnRotation = Quaternion.identity;        // Make sure spawn rotation is always facing the door

    private Animator _anim;     
    private NavMeshAgent _agent;

    // Create a Controller to assign
    [SerializeField] private AnimatorController _animController;



    void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {

    }

    // Make virtual so it can be overridden.
    protected virtual void Movement()
    {
        // Add individual movement code
    }

    void Die()
    {
        this.gameObject.SetActive(false);   
        // Add different death animations or VFX such as disintegrate
    }
}
