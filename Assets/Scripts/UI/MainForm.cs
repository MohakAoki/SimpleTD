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

    [Header("Wave panel")]
    [SerializeField] private GameObject _wavePanel;
    [SerializeField] private TMP_Text _waveText;

    [Header("Enemy Panel")]
    [SerializeField] private GameObject _enemyPanel;
    [SerializeField] private Image _enemyIcon;
    [SerializeField] private TMP_Text _enemyTitle;
    [SerializeField] private TMP_Text _enemySpeed;
    [SerializeField] private TMP_Text _enemyHealth;
    [SerializeField] private RectTransform _enemyHealthbar;

    [Header("Setting")]
    [SerializeField] private Slider _sfxVol;
    [SerializeField] private Slider _musicVol;


    private Tower _tower;
    private Enemy _enemy;

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

        _sfxVol.onValueChanged.RemoveAllListeners();
        _sfxVol.onValueChanged.AddListener(OnSFXChange);

        _musicVol.onValueChanged.RemoveAllListeners();
        _musicVol.onValueChanged.AddListener(OnMusicChange);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void UpdateMoney(int amount)
    {
        _moneyAmountText.text = amount.ToString();
    }

    public void SetWavePanelVisibility(bool enable)
    {
        _wavePanel.SetActive(enable);
    }
    public void UpdateWave(int current, int total)
    {
        _waveText.text = $"{current}/{total}";
    }

    public void SetEnemyPanelVisibility(bool enable)
    {
        _enemyPanel.SetActive(enable);
        if (!enable)
        {
            _enemy = null;
        }
    }

    public void UpdateEnemyHealth(float percent)
    {
        _enemyHealthbar.localScale = new Vector3(percent, 1, 1);
    }

    public void SetEnemyPanelData(Enemy enemy)
    {
        _enemy = enemy;
        EnemyData enemyData = enemy._selfData;
        _enemyTitle.text = enemyData.name;
        _enemyIcon.sprite = enemyData.icon;
        _enemySpeed.text = $"Speed: {enemyData.speed}";
        _enemyHealth.text = $"Health: {enemyData.health.x} - {enemyData.health.y}";
    }

    private void Update()
    {
        if (_enemy)
        {
            if (_enemy.IsAlive)
            {
                UpdateEnemyHealth(_enemy.GetHealthPercentage());
            }
            else
            {
                SetEnemyPanelVisibility(false);
            }
        }
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
        OnResume();
        GameManager.Instance.ExitLevel();
    }

    private void OnSFXChange(float percent)
    {
        AudioManager.Instance.SetSFXVolume(percent);
    }

    private void OnMusicChange(float percent)
    {
        AudioManager.Instance.SetAudioVolume(percent);
    }
}
