using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Create/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string name;
    public string description;
    public Sprite icon;
    public Vector2 health;
    public float speed;
    public int worth;
}
