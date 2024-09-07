using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private TurretBullet _bullet;

    private FieldOfView _fov;
    private LineRenderer _line;

    private bool _isCooldown;

    private void Start()
    {
        _fov = GetComponent<FieldOfView>();
        _line = GetComponent<LineRenderer>();
        _line.SetPosition(0, _bulletSpawnPoint.position);
        _line.SetPosition(1, _bulletSpawnPoint.position);
    }

    private void Update()
    {
        var target = _fov.FindVisibleTargets();

        if (_fov.CanSeeTarget && !_isCooldown)
        {
            StartCoroutine(ShootProjectile(target.position));
        }

        if (_fov.CanSeeTarget)
        {
            _line.SetPosition(1, target.position);
        }
        else
        {
            _line.SetPosition(1, _bulletSpawnPoint.position);
        }
    }

    private IEnumerator ShootProjectile(Vector3 targetDirection)
    {
        Debug.Log($"target: {targetDirection}");
        var bullet = Instantiate(_bullet, _bulletSpawnPoint.position, Quaternion.identity);

        bullet.SetDirection(targetDirection);

        _isCooldown = true;

        yield return new WaitForSeconds(2f);

        _isCooldown = false;
    }
}
