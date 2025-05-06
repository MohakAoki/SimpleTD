using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public string LevelName;

    [SerializeField] private GameObject[] _levels;

    private int _currentLevelIndex;
    private GameObject _currentLevel;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitWhile(() => GameManager.Instance.IsLoading);
        Init();
    }

    private void Init()
    {
        GameManager.Instance.Money = 100;
        StartLevel(_currentLevelIndex);
    }

    private void StartLevel(int index)
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }

        _currentLevel = Instantiate(_levels[index], transform);
        InputManager.Instance.Init();
        EnemySpawner.Instance.Init();
    }


    public void OnLevelFinish()
    {
        // TODO Show Level Finish UI

        // Spawn Next Level
        _currentLevelIndex++;
        if (_currentLevelIndex >= _levels.Length) // End of chapter
        {
            // Return to menu
        }
        else
        {
            StartLevel(_currentLevelIndex);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
