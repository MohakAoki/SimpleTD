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

    public bool IsLoading { get; private set; }

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UI.Instance.OpenForm<MainMenuForm>();
    }

    private IEnumerator LoadScene(string name)
    {
        LoadingForm loadingForm = UI.Instance.GetForm<LoadingForm>();
        loadingForm.SetProgress(0);
        UI.Instance.OpenForm(loadingForm);

        IsLoading = true;
        AsyncOperation LoadingOp = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);

        while (LoadingOp.isDone)
        {
            loadingForm.SetProgress(LoadingOp.progress);
            yield return null;
        }

        for (int i = 0; i < 90; i++)
        {
            loadingForm.SetProgress(Mathf.Sin(i / 180f * Mathf.PI));
            yield return null;
        }
        IsLoading = false;
        UI.Instance.CloseForm(loadingForm);
    }

    private void UnloadScene()
    {
        UI.Instance.GetForm<MainForm>().SetWavePanelVisibility(false);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadGame(string chapterName)
    {
        StartCoroutine(LoadScene(chapterName));
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
