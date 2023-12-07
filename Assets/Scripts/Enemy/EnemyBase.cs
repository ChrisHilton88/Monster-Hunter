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

    private int _enemyHealth;
    [SerializeField] private int _currentWaypoint;
    [SerializeField] private int _agentSpeed;
    private int _agentStopSpeed = 0;
    private int _agentPointsUponDeath;

    [SerializeField] private bool _hasReachedCurrentWaypoint;
    [SerializeField] private bool _hasFinalWaypointBeenReached;
    [SerializeField] private bool _shouldAgentHide;

    protected NavMeshAgent _agent;
    protected Animator _animator;

    public static Action OnEnemyDeath;

    #region Properties
    protected EnemyStates CurrentState { get { return _currentState; } set { _currentState = value; } }
    protected int EnemyHealth { get { return _enemyHealth; } set { _enemyHealth = value; } }
    protected int CurrentWaypoint { get { return _currentWaypoint; } set { _currentWaypoint = value; } }
    protected int AgentSpeed { get { return _agentSpeed; } set { _agentSpeed = value; } }   
    protected int AgentStopSpeed { get { return _agentStopSpeed; } }
    protected int AgentPointsUponDeath { get { return _agentPointsUponDeath; } set { _agentPointsUponDeath = value; } }
    protected bool HasReachedCurrentWaypoint { get { return _hasReachedCurrentWaypoint; } set { _hasReachedCurrentWaypoint = value; } }
    protected bool HasFinalWaypointBeenReached { get { return _hasFinalWaypointBeenReached; } set { _hasFinalWaypointBeenReached = value; } }
    protected bool ShouldAgentHide { get { return _shouldAgentHide; } set { _shouldAgentHide = value; } }
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
        UIManager.Instance.UpdateEnemyCount();  
    }

    protected virtual void OnDisable()
    {
        SpawnManager.globalInternalEnemyCount--;
        UIManager.Instance.UpdateEnemyCount();
    }

    protected virtual void GrabComponents()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Initialisation()
    {
        SpawnManager.globalInternalEnemyCount++;
        CurrentState = EnemyStates.Idle;     // Always set initial state to run
        CurrentWaypoint = 0;    // Set all enemies first waypoint to be equal to 0.
        //_agent.speed = AgentStopSpeed;
        _agent.Warp(SpawnManager.Instance.SpawnPoint.position);
        HasReachedCurrentWaypoint = true;      // Set to true at start it doesn't choose a new position immediately
        ShouldAgentHide = false;
        StartMoving();
    }
    #endregion

    #region Methods
    protected abstract void FixedUpdate();

    protected virtual void CheckState()       // All enemies have individual behaviour/states (possibly) - Could be some common behaviour in some states
    {
        switch (CurrentState)
        {
            case EnemyStates.EndPoint:
                // Enemy plays banging animation to show they are damaging the door
                break;
            case EnemyStates.Hide:
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

    protected virtual void StartMoving()
    {
        _agent.SetDestination(WaypointManager.Instance.Waypoints[CurrentWaypoint].position);
        CurrentState = EnemyStates.Run;
        _agent.speed = AgentSpeed;
        HasReachedCurrentWaypoint = false;
    }

    // Responsible for checking the distance to the next waypoint and calling the appropriate method
    protected virtual void CheckAgentDistanceToWaypoint()
    {
        //while (_agent.pathPending)
        //    return;

        if (!HasReachedCurrentWaypoint && !_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            // Agent reaches destination and checks whether they should hide
            ShouldAgentHide = WaypointManager.Instance.ShouldEnemyHideAtNewDestination();
            HasReachedCurrentWaypoint = true;

            // Agent reaches their new destination, they should now hide if TRUE
            if (CurrentWaypoint < 12 && ShouldAgentHide)
            {
                StartCoroutine(HidingRoutine());
            }
            else if (CurrentWaypoint < 12 && !ShouldAgentHide)
            {
                SetNewAgentDestination();
            }
            else
            {
                SetAgentFinalDestination();     // Set to last waypoint
            }
        }
    }

    // Responsible for setting a new destination for the enemy agent
    protected virtual void SetNewAgentDestination()
    {
        StartCoroutine(SetNewAgentDestinationRoutine());
    }

    // Responsible for setting the agents final destination and hadling any behaviour
    protected virtual void SetAgentFinalDestination()
    {
        StartCoroutine(SetAgentFinalDestinationRoutine());
    }

    // Responsible for handling what happens to the enemy when they die
    protected virtual void Die()
    {
        OnEnemyDeath?.Invoke();
        ScoreManager.Instance.AddPointsToScore(AgentPointsUponDeath);
        CurrentState = EnemyStates.Death;
        _animator.SetTrigger("IsDead");
        ResetPositionOnDeath();
    }

    // Responsible for resetting the game objects position in World Space when dying and disabling itself
    protected virtual void ResetPositionOnDeath()
    {
        _agent.speed = AgentStopSpeed;
        _agent.Warp(SpawnManager.Instance.SpawnPoint.position);     // Set agent position back to spawn pos
        gameObject.SetActive(false);
    }
    #endregion

    #region Coroutines
    // Responsible for the hiding routine
    protected IEnumerator HidingRoutine()
    {
        int newTimeToHide = WaypointManager.Instance.RandomTimeToHide();
        _agent.speed = AgentStopSpeed;
        CurrentState = EnemyStates.Hide;
        yield return new WaitForSeconds(newTimeToHide);
        ShouldAgentHide = false;
        HasReachedCurrentWaypoint = false;
        _agent.speed = AgentSpeed;
        CurrentState = EnemyStates.Run;
    }

    protected IEnumerator SetNewAgentDestinationRoutine()
    {
        Transform newWaypoint = WaypointManager.Instance.GetNextWaypoint(CurrentWaypoint, out int nextWaypoint);
        CurrentWaypoint = nextWaypoint;
        _agent.SetDestination(newWaypoint.position);

        while (_agent.pathPending)
            yield return null;

        HasReachedCurrentWaypoint = false;
    }

    protected IEnumerator SetAgentFinalDestinationRoutine()
    {
        while (_agent.pathPending)
            yield return null;

        if (_agent.remainingDistance < 0.5f)
        {
            HasFinalWaypointBeenReached = true;
            ShouldAgentHide = false;
            _agent.speed = AgentStopSpeed;
            CurrentState = EnemyStates.EndPoint;
            // TODO: Play destroy animation
        }
    }
    #endregion
}
