using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<Unit> _units = new List<Unit>();
    [SerializeField] private UnitUI _unitUI;

    private int _currentIndex = 0;

    private void Start()
    {
        _unitUI = UIManager.Instance.UnitUI;

        _unitUI.CreateChildElements(_units);

        foreach (var unit in _units)
        {
            unit.OnSurvived.AddListener(NextUnit);
            unit.OnDeath.AddListener(DeathUnit);
        }

        _units[_currentIndex].IsSelected = true;
    }

    private void DeathUnit()
    {
        LevelManager.Instance.RestartCurrentLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NextUnit();
        }
    }

    private void NextUnit()
    {
        if (_units.All(unit => unit.Survive))
        {
            LevelManager.Instance.NextLevel();
            return;
        }

        _units[_currentIndex].IsSelected = false;

        do
        {
            _currentIndex++;

            if (_currentIndex >= _units.Count)
            {
                _currentIndex = 0;
            }
        }
        while (_units[_currentIndex].Survive);

        _units[_currentIndex].IsSelected = true;
    }
}
