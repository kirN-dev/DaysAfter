using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UnitCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Image _selectedImage;
    [SerializeField] private HealthBar _healthBar;

    private Unit _unit;
    public Unit Unit => _unit;
    public bool IsDestroy { get; private set; }

    public void Select()
    {
        _selectedImage.enabled = true;
    }

    public void Deselect()
    {
        if (_selectedImage is null)
        {
            return;
        }

        _selectedImage.enabled = false;
    }

    public void SetNumber(int number)
    {
        _textMeshPro.text = number.ToString();
    }
    
    public void SetUnit(Unit unit)
    {
        _unit = unit;
        _healthBar.SetMaxHeal(_unit.MaxHealth);
        _healthBar.SetCurrentHeal(_unit.CurrentHealth);

        _unit.OnHealthChanged.AddListener(_healthBar.SetCurrentHeal);
        _unit.OnSurvived.AddListener(Survive);
        _unit.OnSelected.AddListener(Select);
        _unit.OnDeselected.AddListener(Deselect);
    }

    public void SelectUnit()
    {
        UIManager.Instance.UnitUI.DeselecAll();
        _unit.IsSelected = true;
    }

    private void Survive()
    {
        Deselect();
        IsDestroy = true;
        Destroy(gameObject);
    }
}
