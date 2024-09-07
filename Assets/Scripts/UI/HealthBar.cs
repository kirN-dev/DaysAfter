using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private int _currentHealth;
    private int _maxHealth;

    public void SetCurrentHeal(int health)
    {
        if (0 > health)
        {
            Debug.Log("Current health value connot be less than zero");
            return;
        }

        _currentHealth = health;
        _slider.value = health;
    }
    
    public void SetMaxHeal(int health)
    {
        if (0 > health)
        {
            Debug.Log("Max health value connot be less than zero");
            return;
        }

        if (_currentHealth < health)
        {
            _currentHealth = health;
        }

        _maxHealth = health;
        _slider.maxValue = health;
    }
    
}
