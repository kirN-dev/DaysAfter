using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float _viewRadius = 5f; // Радиус зрения противника
    [Range(0, 360)]
    [SerializeField] private float _viewAngle = 120f; // Угол зрения противника

    [SerializeField] private LayerMask _targetMask; // Маска юнитов, которых нужно искать
    [SerializeField] private LayerMask _obstacleMask; // Маска препятствий, которые блокируют видимость

    [SerializeField] private int _meshResolution = 50; // Количество лучей для поля зрения


    private Transform _target; // Цель, которую будет преследовать противник
    private Mesh _viewMesh; // Mesh для поля зрения

    public bool CanSeeTarget => _target != null;
    public Transform Target => _target;

    void Start()
    {
        var mesh = transform.GetComponent<MeshRenderer>();

        _viewMesh = new Mesh()
        {
            name = "View Mesh"
        };

        var meshFilter = transform.GetComponent<MeshFilter>();
        meshFilter.mesh = _viewMesh;
        
    }

    // Поиск видимых целей
    public Transform FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider targetCollider in targetsInViewRadius)
        {
            Transform targetTransform = targetCollider.transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

            // Проверяем, находится ли цель в поле зрения
            if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
                // Проверка на наличие препятствий
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                {
                    // Если цель ближе, чем предыдущая, делаем ее целью
                    if (distanceToTarget < closestDistance)
                    {
                        closestDistance = distanceToTarget;
                        target = targetTransform;
                    }
                }
            }
        }

        _target = target;

        return target;
    }

    // Отрисовка поля зрения
    public void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(_viewAngle * _meshResolution);
        float stepAngleSize = _viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - _viewAngle / 2 + stepAngleSize * i;

            Vector3 point = ViewCastObstacle(angle);

            viewPoints.Add(point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    public void ClearFieldOfView()
    {

        _viewMesh.Clear();
    }

    private Vector3 ViewCastObstacle(float angle)
    {
        Vector3 direction = DirectionFromAngle(angle, true);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _viewRadius, _obstacleMask))
        {
            return hit.point;
        }
        else
        {
            return transform.position + direction * _viewRadius;
        }
    }

    // Вспомогательная функция для вычисления направления на основе угла
    private Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // Отрисовка в редакторе Unity для наглядности
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Vector3 leftBoundary = DirectionFromAngle(-_viewAngle / 2, false);
        Vector3 rightBoundary = DirectionFromAngle(_viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * _viewRadius);

        if (CanSeeTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Target.position);
        }
    }
}