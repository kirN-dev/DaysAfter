using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private UnitManager _unitManager;

    private List<Unit> _exitUnit = new List<Unit>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Unit>(out var unit))
        {
            unit.Survive = true;
        }
    }
}
