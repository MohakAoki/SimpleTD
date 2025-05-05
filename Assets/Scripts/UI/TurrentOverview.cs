using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurrentOverview : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _details;
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private Image icon;

    private Button _selfButton;

    public void SetData(TowerData data)
    {
        _title.text = data.name;
        _description.text = data.description;
        float dps = data.damage / data.recoilUpgrades[0].recoilSpeed;
        _details.text = $"Range - DPS - Speed\n{data.rangeUpgrades[0].range} - {dps} - {data.rotationUpgrades[0].rotationSpeed}";
        _cost.text = data.unlockCost.ToString();

        icon.sprite = data.icon;

        _selfButton = GetComponent<Button>();
        _selfButton.onClick.RemoveAllListeners();
        _selfButton.onClick.AddListener(() => InputManager.Instance.InstallTower(data));
    }
}
