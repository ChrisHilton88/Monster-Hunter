using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Responsibility - Base class blueprint for all inheriting enemies 
public abstract class EnemyBase : MonoBehaviour
{
    protected enum EnemyStates
    {
        Run,
        Hide,
        Death
    }

    [SerializeField] private EnemyStates _currentState;

    [SerializeField] private int _enemyHealth;
    [SerializeField] private int _currentWaypoint;

    private bool _hasNewDestinationBeenFound;
    [SerializeField] private bool _hasFinalWaypointBeenReached;

    protected NavMeshAgent _agent;
    protected Animator _animator;

    #region Properties
    protected EnemyStates CurrentState { get { return _currentState; } set { _currentState = value; } }
    protected int EnemyHealth { get { return _enemyHealth; } set { _enemyHealth = value; } }
    protected int CurrentWaypoint { get { return _currentWaypoint; } set { _currentWaypoint = value; } }
    protected bool HasNewDestinationBeenFound { get { return _hasNewDestinationBeenFound; } set { _hasNewDestinationBeenFound = value; } }
    protected bool HasFinalWaypointBeenReached { get { return _hasFinalWaypointBeenReached; } set { _hasFinalWaypointBeenReached = value; } }   
    #endregion



    #region Initialisation
    protected virtual void Awake()
    {
        GrabComponents();
    }

    // Because these gameobjects are using object pooling and will be enabled/disabled alot, it is better to have this initialisation here instead of Start()
    protected virtual void OnEnable()
    {
        Initialisation();
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Start()
    {
        Initialisation();
    }

    protected virtual void GrabComponents()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Initialisation()
    {
        CurrentState = EnemyStates.Run;     // Always set initial state to run
        CurrentWaypoint = 0;    // Set all enemies first waypoint to be equal to 0.
        _agent.speed = 0;
        HasNewDestinationBeenFound = true;
        _agent.SetDestination(WaypointManager.Instance.Waypoints[CurrentWaypoint].position);
        StartCoroutine(StartMoving());
    }
    #endregion

    protected abstract void FixedUpdate();

    protected virtual void CheckState()       // All enemies have individual behaviour/states (possibly) - Could be some common behaviour in some states
    {
        switch (CurrentState)
        {
            case EnemyStates.Run:
                CheckAgentDistanceToWaypoint();
                break;
            case EnemyStates.Hide:
                // Set agent speed to 0
                // Pause for a random amount of time - Idle animation/crouch to hide behind boxes
                // Choose a new waypoint and resume movement
                break;
            case EnemyStates.Death:
                // Enemy stops moving and falls over
                // Disable collider so we can't continue to shoot the enemy
                // Reset back into the pool
                break;
            default:
                break;
        }
    }

    // How can we return/bring back the updated position
protected virtual void CheckAgentDistanceToWaypoint()
    {
        if(!HasNewDestinationBeenFound && _agent.remainingDistance < 0.5f)
        {
            if (CurrentWaypoint < 12)
            {
                HasNewDestinationBeenFound = true;
                SetNewAgentDestination();
                StartCoroutine(WaitTimerToUpdateDestinationBool());
            }
            else
            {
                SetFinalAgentDestination();
            }
        }
    }

    protected virtual void SetNewAgentDestination()
    {
        Transform newWaypoint = WaypointManager.Instance.GetNextWaypoint(CurrentWaypoint, out int nextWaypoint);
        CurrentWaypoint = nextWaypoint;
        _agent.SetDestination(newWaypoint.position);
    }

    protected virtual void SetFinalAgentDestination()
    {
        HasFinalWaypointBeenReached = true;
        _agent.speed = 0;
        //_agent.SetDestination()
    }


    protected virtual void Die()
    {
        _animator.SetTrigger("IsDead");
    }

    protected IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(1f);
        _agent.speed = 3.5f;
        HasNewDestinationBeenFound = false;
    }

    protected IEnumerator WaitTimerToUpdateDestinationBool()
    {
        yield return new WaitForSeconds(2f);
        HasNewDestinationBeenFound = false;
    }
}
