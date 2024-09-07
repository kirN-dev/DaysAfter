using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : MonoBehaviour
{
    [SerializeField] private bool _visibleFOV;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private int _attackDamage = 3;

    private NavMeshAgent _agent;
    private EnemyMover _enemyMover;
    private FieldOfView _fov;
    private Transform _target;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    void Start()
    {
        _fov = GetComponent<FieldOfView>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyMover = new EnemyMover(_agent);
        _enemyMover.Speed = _moveSpeed;

        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    void Update()
    {
        _target = _fov.FindVisibleTargets();

        if (_fov.CanSeeTarget)
        {
            ChaseTarget();
        }
        else
        {
            ReturnToStart();
        }

        if (_visibleFOV)
        {
            _fov.DrawFieldOfView(); // Отрисовываем поле зрения каждый кадр
        }
        else
        {
            _fov.ClearFieldOfView();
        }
    }

    // Преследование цели
    private void ChaseTarget()
    {
        _enemyMover.Move(_target.position);
    }
    
    private void ReturnToStart()
    {
        _enemyMover.Move(_startPosition);

        if (_enemyMover.IsStopped)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _startRotation, Time.deltaTime * 2f);
        }
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
