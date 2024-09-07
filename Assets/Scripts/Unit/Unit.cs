using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Image _arrow;
    private UnitMover _mover;
    private Vector3 _moveDirection;

    private UnitInput _input;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            _arrow.enabled = value;
            if (value)
            {
                OnSelected.Invoke();
            }
            else
            {
                OnDeselected.Invoke();

            }
        }
    }

    public int Number { get; set; }

    private bool _survive;
    public bool Survive
    {
        get => _survive;
        set
        {
            _survive = value;
            IsSelected = false;
            OnSurvived.Invoke();
            gameObject.SetActive(false);
        }
    }

    [field: SerializeField]
    public int MaxHealth { get; set; } = 10;
    public int CurrentHealth { get; set; }

    public UnityEvent<int> OnHealthChanged;

    public UnityEvent OnDeath;

    public UnityEvent OnSurvived;

    public UnityEvent OnSelected;

    public UnityEvent OnDeselected;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        _mover = GetComponent<UnitMover>();
        _input = new UnitInput();
    }

    private void Update()
    {
        _moveDirection = _input.Unit.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!IsSelected)
        {
            return;
        }

        _mover.Move(_moveDirection);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            OnDeath.Invoke();
        }

        OnHealthChanged.Invoke(CurrentHealth);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
