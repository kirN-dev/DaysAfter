using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TurretBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private int _attackDamage = 1;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector3 direction)
    {
        transform.LookAt(direction);
        _rb.velocity = transform.forward * _speed;
    }
    public void SetDamage(int damage)
    {
        _attackDamage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent<Unit>(out var unit))
        {
            unit.TakeDamage(_attackDamage);
        }

        Destroy(gameObject);
    }
}
