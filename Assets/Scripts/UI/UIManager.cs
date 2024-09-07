using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;

    [SerializeField] private UnitUI _unitUI;

    public UnitUI UnitUI => _unitUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        restartButton.onClick.AddListener(RestartLevel);
        nextLevelButton.onClick.AddListener(NextLevel);

        LevelManager.Instance.OnNextLevel.AddListener(UnitUI.RemoveChildsElement);
        LevelManager.Instance.OnRestartLevel.AddListener(UnitUI.RemoveChildsElement);
    }

    public void UpdateLevelText(int currentLevel, int totalLevels)
    {
        levelText.text = $"Уровень {currentLevel + 1} / {totalLevels}";
    }

    private void RestartLevel()
    {
        LevelManager.Instance.RestartCurrentLevel();
    }

    private void NextLevel()
    {
        LevelManager.Instance.NextLevel();
    }

    public void ShowNextLevelButton(bool show)
    {
        nextLevelButton.gameObject.SetActive(show);
    }
}