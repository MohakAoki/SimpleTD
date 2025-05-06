using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _money;
    public int Money
    {
        set
        {
            _money = value;
            UI.Instance.GetForm<MainForm>().UpdateMoney(value);
        }
        get { return _money; }
    }

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        yield return LoadScene("level_mars");
    }

    private IEnumerator LoadScene(string name)
    {
        AsyncOperation LoadingOp = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);

        while (LoadingOp.isDone)
        {
            yield return null;
        }
    }

    private void UnloadScene()
    {
        UI.Instance.GetForm<MainForm>().SetWavePanelVisibility(false);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        UnloadScene();
        StartCoroutine(LoadScene("level_mars"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
