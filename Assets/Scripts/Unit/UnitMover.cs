using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _rotationSpeed = 20f;
    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void ForceStop()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void Move(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);


        if (moveDirection == Vector3.zero)
        {
            ForceStop();
            return;
        }

        _rb.AddForce(moveDirection * _moveSpeed, ForceMode.Force);

        Quaternion toRotation = Quaternion.LookRotation(moveDirection);
        _rb.rotation = Quaternion.Slerp(_rb.rotation, toRotation, _rotationSpeed * Time.fixedDeltaTime);
    }
}
