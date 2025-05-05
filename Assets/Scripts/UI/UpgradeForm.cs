using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeForm : Form
{
    [SerializeField] private TMP_Text _baseCurrent;
    [SerializeField] private TMP_Text _mountCurrent;
    [SerializeField] private TMP_Text _headCurrent;

    [SerializeField] private TMP_Text _baseUpgrade;
    [SerializeField] private TMP_Text _mountUpgrade;
    [SerializeField] private TMP_Text _headUpgrade;

    [SerializeField] private TMP_Text _baseCost;
    [SerializeField] private TMP_Text _mountCost;
    [SerializeField] private TMP_Text _headCost;

    [SerializeField] private Button _baseButton;
    [SerializeField] private Button _mountButton;
    [SerializeField] private Button _headButton;
    [SerializeField] private Button _exitButton;


    private Tower _tower;

    public override void Init()
    {
        _exitButton.onClick.RemoveAllListeners();
        _exitButton.onClick.AddListener(Close);

        _baseButton.onClick.RemoveAllListeners();
        _baseButton.onClick.AddListener(OnBaseUpgrade);

        _headButton.onClick.RemoveAllListeners();
        _headButton.onClick.AddListener(OnHeadUpgrade);

        _mountButton.onClick.RemoveAllListeners();
        _mountButton.onClick.AddListener(OnMountUpgrade);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
        InputManager.Instance.SelectTower(null);
        _tower = null;
    }

    public void SetData(TowerData data, Tower currentTower)
    {
        _tower = currentTower;

        _headCurrent.text = currentTower.GetDPS().ToString();
        _baseCurrent.text = currentTower.GetRange().ToString();
        _mountCurrent.text = currentTower.GetRotationSpeed().ToString();

        _headUpgrade.text = currentTower.GetNextDPS().ToString();
        _baseUpgrade.text = currentTower.GetNextRange().ToString();
        _mountUpgrade.text = currentTower.GetNextRotationSpeed().ToString();

        int headCost = currentTower.GetNextDPSCost();
        int baseCost = currentTower.GetNextRangeCost();
        int mountCost = currentTower.GetNextRotationSpeedCost();

        if (headCost > 0)
        {
            _headCost.text = headCost.ToString();
            _headButton.gameObject.SetActive(true);
        }
        else
        {
            _headCost.text = "Max level";
            _headButton.gameObject.SetActive(false);
        }

        if (baseCost > 0)
        {
            _baseCost.text = baseCost.ToString();
            _baseButton.gameObject.SetActive(true);
        }
        else
        {
            _baseCost.text = "Max level";
            _baseButton.gameObject.SetActive(false);
        }

        if (mountCost > 0)
        {
            _mountCost.text = mountCost.ToString();
            _mountButton.gameObject.SetActive(true);
        }
        else
        {
            _mountCost.text = "Max level";
            _mountButton.gameObject.SetActive(false);
        }
    }

    private void OnBaseUpgrade()
    {
        _tower.UpgradeBase();
        SetData(_tower.SelfData, _tower);
        InputManager.Instance.SelectTower(_tower);
    }
    private void OnMountUpgrade()
    {
        _tower.UpgradeMount();
        SetData(_tower.SelfData, _tower);
        InputManager.Instance.SelectTower(_tower);
    }
    private void OnHeadUpgrade()
    {
        _tower.UpgradeHead();
        SetData(_tower.SelfData, _tower);
        InputManager.Instance.SelectTower(_tower);
    }
}
