using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelPrefabs;

    private GameObject currentLevel;
    private int currentLevelIndex = -1;
    public static LevelManager Instance { get; private set; }

    public UnityEvent OnRestartLevel;
    public UnityEvent OnNextLevel;

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
        NextLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelPrefabs.Count)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        if (currentLevel != null)
        {
            Destroy(currentLevel);
        }
        UIManager.Instance.UpdateLevelText(levelIndex, levelPrefabs.Count);
        currentLevelIndex = levelIndex;
        currentLevel = Instantiate(levelPrefabs[levelIndex]);
    }

    public void RestartCurrentLevel()
    {
        if (currentLevelIndex != -1)
        {
            OnRestartLevel.Invoke();
            StartLevel(currentLevelIndex);
        }
        else
        {
            Debug.LogWarning("No level to restart!");
        }
    }

    public void NextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex < levelPrefabs.Count)
        {
            OnNextLevel.Invoke();
            StartLevel(nextLevelIndex);
        }
        else
        {
            Debug.Log("Это был последний уровень!");
        }
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public int GetLevelCount()
    {
        return levelPrefabs.Count;
    }
}