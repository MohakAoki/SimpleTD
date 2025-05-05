using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainForm : Form
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private TMP_Text _moneyAmountText;


    private Tower _tower;

    public override void Init()
    {
        _pauseButton.onClick.RemoveAllListeners();
        _pauseButton.onClick.AddListener(OnPause);

        _resumeButton.onClick.RemoveAllListeners();
        _resumeButton.onClick.AddListener(OnResume);

        _restartButton.onClick.RemoveAllListeners();
        _restartButton.onClick.AddListener(OnRestart);

        _exitButton.onClick.RemoveAllListeners();
        _exitButton.onClick.AddListener(OnExit);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnPause()
    {
        if (Time.timeScale == 0)
        {
            OnResume();
        }
        else
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);
        }
    }

    private void OnResume()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnRestart()
    {
        OnResume();
        GameManager.Instance.RestartLevel();
    }

    private void OnExit()
    {
        GameManager.Instance.ExitGame();
    }

    public void UpdateMoney(int amount)
    {
        _moneyAmountText.text = amount.ToString();
    }
}
