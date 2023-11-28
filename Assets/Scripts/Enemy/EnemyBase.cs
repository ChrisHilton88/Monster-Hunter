using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Responsibility - Base class blueprint for all inheriting enemies 
public abstract class EnemyBase : MonoBehaviour
{
    protected enum EnemyStates
    {
        Death,
        EndPoint,
        Hide,           // Only used as an intro state when starting the game and reenabling from object pool
        Idle,
        Run
    }

    [SerializeField] private EnemyStates _currentState;

    [SerializeField] private int _enemyHealth;
    [SerializeField] private int _currentWaypoint;

    [SerializeField] private bool _hasNewDestinationBeenFound;
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

    private static Action<int> OnDeath;         // Pass in a points value on death


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
        CurrentState = EnemyStates.Idle;     // Always set initial state to run
        CurrentWaypoint = 0;    // Set all enemies first waypoint to be equal to 0.
        _agent.speed = 0;
        HasNewDestinationBeenFound = true;
        _agent.SetDestination(WaypointManager.Instance.Waypoints[CurrentWaypoint].position);
        //StartCoroutine(StartMoving());
    }
    #endregion

    protected abstract void FixedUpdate();

    protected virtual void CheckState()       // All enemies have individual behaviour/states (possibly) - Could be some common behaviour in some states
    {
        switch (CurrentState)
        {
            case EnemyStates.EndPoint:
                // Enemy plays banging animation to show they are damaging the door
                break;
            case EnemyStates.Hide:
                // Set agent speed to 0
                // Pause for a random amount of time - Idle animation/crouch to hide behind boxes
                // Choose a new waypoint and resume movement
                // Hide/Idle animation
                break;
            case EnemyStates.Idle:
                // Play Idle animation 
                // Make sure enemy speed is 0
                break;
            case EnemyStates.Run:
                CheckAgentDistanceToWaypoint();
                // Run animation
                break;
            default:
                break;
        }
    }

    // Responsible for checking the distance to the next waypoint and calling the appropriate method
    protected virtual void CheckAgentDistanceToWaypoint()
    {
        if(!HasNewDestinationBeenFound && _agent.remainingDistance < 0.5f)
        {
            if(CurrentWaypoint < 12)
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

    // Responsible for setting a new destination for the enemy agent
    protected virtual void SetNewAgentDestination()
    {
        Transform newWaypoint = WaypointManager.Instance.GetNextWaypoint(CurrentWaypoint, out int nextWaypoint);
        CurrentWaypoint = nextWaypoint;
        _agent.SetDestination(newWaypoint.position);
    }

    // Responsible for setting the agents final destination and hadling any behaviour
    protected virtual void SetFinalAgentDestination()
    {
        CurrentState = EnemyStates.EndPoint;
        HasFinalWaypointBeenReached = true;
        _agent.speed = 0;
    }

    // Responsible for handling what happens to the enemy when they die
    protected virtual void Die()
    {
        CurrentState = EnemyStates.Death;
        _animator.SetTrigger("IsDead");
        _agent.speed = 0;
        // Disable enemy game object so as to returning it to the object pool
        ResetPositionOnDeath();
    }

    // Responsible for resetting the game objects position in World Space when dying and disabling itself
    protected virtual void ResetPositionOnDeath()
    {
        _agent.Warp(SpawnManager.Instance.SpawnPoint.position);     // Set agent position back to spawn pos
        CurrentState = EnemyStates.Idle;        // Set to idle for a short time before commencing 
        gameObject.SetActive(false);
    }

    // Responsible for telling the agent they can start moving at the start of the game
    protected IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(1f);
        CurrentState = EnemyStates.Run;
        _agent.speed = 3.5f;
        HasNewDestinationBeenFound = false;
    }

    // Responsible for handling the timer between choosing waypoint destinations
    protected IEnumerator WaitTimerToUpdateDestinationBool()
    {
        yield return new WaitForSeconds(2f);
        HasNewDestinationBeenFound = false;
    }
}
