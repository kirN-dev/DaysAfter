using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private int _attackDamage = 5;

    private int currentPatrolPointIndex = 0;
    private EnemyMover _enemyMover;
    private bool isWaiting = false;

    private void Start()
    {
        var agent = GetComponent<NavMeshAgent>();
        _enemyMover = new EnemyMover(agent);
        _enemyMover.Speed = patrolSpeed;

        StartPatrol();
    }

    private void Update()
    {
        if (!isWaiting && _enemyMover.IsStopped)
        {
            isWaiting = true;
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    private void StartPatrol()
    {
        Vector3 moveDirection = patrolPoints[currentPatrolPointIndex].position;

        _enemyMover.Move(moveDirection);
    }

    private System.Collections.IEnumerator WaitAtPatrolPoint()
    {
        yield return new WaitForSeconds(waitTime);
        currentPatrolPointIndex = ++currentPatrolPointIndex % patrolPoints.Length;
        StartPatrol();
        isWaiting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Unit>(out var unit))
        {
            unit.TakeDamage(_attackDamage);
            Destroy(gameObject);
        }
    }
}
