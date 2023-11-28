using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WaypointManager : MonoSingleton<WaypointManager>
{
    private int _lastWaypoint;              // Sets last waypoint in waypoint List
    private int _minDiceRoll = 0, _maxDiceRoll = 101;       //  Range between which a dice roll can occur.
    private int _diceRollDivider = 30;          // Dividing line to max decisions

    [SerializeField] private List<Transform> _waypoints = new List<Transform>();            // List of waypoints for enemies


    #region Properties
    public int LastWayPoint { get { return _lastWaypoint; } private set { _lastWaypoint = value; } }
    public List<Transform> Waypoints { get {  return _waypoints; } }
    #endregion


    void Start()
    {
        LastWayPoint = _waypoints.Count - 1;
    }

    // Responsible for the enemy calling into and choosing their next waypoint in the List (randomly) and returning the nextWaypoint
    public Transform GetNextWaypoint(int currentWaypoint, out int nextWaypoint)
    {
        if(currentWaypoint < 12)        
        {
            int randomNumber = RandomNumberGenerator(currentWaypoint, LastWayPoint);
            Transform newWaypoint = _waypoints[randomNumber];
            nextWaypoint = randomNumber;
            return newWaypoint;
        }
        else
        {
            nextWaypoint = 12;
            return _waypoints[LastWayPoint];
        }
    }

    // Responsible for generating a new randon number to pass into the GetNextWaypoint() method
    int RandomNumberGenerator(int minNumber, int maxNumber)     
    {
        int newRandomNumber = Random.Range(minNumber + 1, maxNumber);       
        return newRandomNumber;
    }

    // Responsible for deciding whether the enemy should hide or not at the next destination
    bool ShouldEnemyHideAtNewDestination()
    {
        int newRandomNumber = Random.Range(_minDiceRoll, _maxDiceRoll);

        if(newRandomNumber <= _diceRollDivider)         // <= 30
            return true;
        else return false;
    }

    // Responsible for deciding how long the enemy agent should hide for 
    int RandomTimeToHide()
    {
        int newRandomNumber = Random.Range(3, 8);
        return newRandomNumber;
    }
}
