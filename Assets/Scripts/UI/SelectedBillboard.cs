using UnityEngine;

public class SelectedBillboard : MonoBehaviour
{
    [SerializeField] private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.forward);
    }
}
