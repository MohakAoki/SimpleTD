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

    public void LoadGame(string chapterName)
    {
        StartCoroutine(LoadScene(chapterName));
    }

    public void RestartLevel()
    {
        StartCoroutine(UnloadScene());
        StartCoroutine(LoadScene("level_mars"));
    }

    public void ExitLevel()
    {
        StartCoroutine(UnloadScene());
        SceneManager.LoadScene("main_menu");
    }

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UI.Instance.OpenForm<MainMenuForm>();
        SceneManager.LoadScene("main_menu");
    }

    private IEnumerator LoadScene(string name)
    {
        LoadingForm loadingForm = UI.Instance.GetForm<LoadingForm>();
        loadingForm.SetProgress(0);
        loadingForm.SetText("Loading ...");
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
        UI.Instance.CloseForm<MainMenuForm>();
    }

    private IEnumerator UnloadScene()
    {
        UI.Instance.CloseForm<MainForm>();

        IsLoading = true;
        AsyncOperation unloadingOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        LoadingForm loadingForm = UI.Instance.GetForm<LoadingForm>();
        loadingForm.SetProgress(1);
        loadingForm.SetText("... Unloading");
        UI.Instance.OpenForm(loadingForm);
        GlobalPool.Instance.FreePool();

        for (int i = 90; i > 0; i--)
        {
            loadingForm.SetProgress(Mathf.Sin(i / 180f * Mathf.PI));
            yield return null;
        }
        IsLoading = false;
        UI.Instance.CloseForm(loadingForm);
        UI.Instance.OpenForm<MainMenuForm>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
