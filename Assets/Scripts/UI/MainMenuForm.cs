using UnityEngine;
using UnityEngine.UI;

public class MainMenuForm : Form
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    public override void Init()
    {
        _startButton.onClick.RemoveAllListeners();
        _startButton.onClick.AddListener(StartGame);

        _exitButton.onClick.RemoveAllListeners();
        _exitButton.onClick.AddListener(() => Application.Quit());
    }

    public override void Open()
    {
        gameObject.SetActive(true);

        AudioManager.Instance.PlayMusic("main_menu", 2, true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    private void StartGame()
    {
        GameManager.Instance.LoadGame("level_mars");
        UI.Instance.CloseForm(this);
    }
}
