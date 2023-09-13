using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoSingleton<WaypointManager>
{
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();


    void Start()
    {
        
    }

    public Transform GetNextWaypoint()
    {
        Transform newWaypoint = null;

        // Use the list to find a new random waypoint number
        // Number must be higher than the current waypoint

        return newWaypoint; 
    }
}
