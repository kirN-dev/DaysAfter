using UnityEngine;
using UnityEngine.AI;

public class EnemyMover
{
    private NavMeshAgent _agent;

    public bool IsStopped => _agent.remainingDistance <= _agent.stoppingDistance;
    public float Speed 
    {
        get => _agent.speed;
        set => _agent.speed = value;
    }

    public EnemyMover(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public void Move(Vector3 direction)
    {
        _agent.SetDestination(direction);
    }
}
