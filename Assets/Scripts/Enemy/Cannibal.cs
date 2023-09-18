using UnityEngine;
using UnityEngine.AI;

public class Cannibal : EnemyBase
{
    NavMeshAgent _agent;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        //_agent.destination = SpawnManager.Instance.EndPoint.position;
    }
}
