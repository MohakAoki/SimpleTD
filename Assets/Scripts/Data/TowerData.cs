using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Data", menuName = "Create/Tower Data")]
public class TowerData : ScriptableObject
{
    public string name;
    public string description;
    public Sprite icon;
    public Tower prefab;
    public Bullet bulletPrefab;

    public float damage;
    public int unlockCost;

    public TowerRotationUpgrade[] rotationUpgrades;
    public TowerRecoilUpgrade[] recoilUpgrades;
    public TowerRangeUpgrade[] rangeUpgrades;
}

[Serializable]
public class TowerRotationUpgrade
{
    public int upgradeCost;
    public float rotationSpeed;
    public Material Material;
}

[Serializable]
public class TowerRecoilUpgrade
{
    public int upgradeCost;
    public float recoilSpeed;
    public Material Material;
}

[Serializable]
public class TowerRangeUpgrade
{
    public int upgradeCost;
    public float range;
    public Material Material;
}
