using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoSingleton<WaypointManager>
{
    private int _lastWaypoint;

    [SerializeField] private List<Transform> _waypoints = new List<Transform>();

    #region Properties
    public int LastWayPoint { get { return _lastWaypoint; } private set { _lastWaypoint = value; } }
    public List<Transform> Waypoints { get {  return _waypoints; } }
    #endregion


    void Start()
    {
        LastWayPoint = _waypoints.Count - 1;
    }

    // Responsible for the enemy calling into and choosing their next waypoint in the List (randomly)
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
            nextWaypoint = 0;
            Debug.Log("Last Waypoint");
            return _waypoints[LastWayPoint];
        }
    }

    // Responsible for generating a new randon number to pass into the GetNextWaypoint() method
    int RandomNumberGenerator(int minNumber, int maxNumber)     
    {
        int newRandomNumber = Random.Range(minNumber + 1, maxNumber);       
        return newRandomNumber;
    }
}
