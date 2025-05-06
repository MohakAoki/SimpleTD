using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float _maxHealth;
    protected float _moveSpeed;
    protected float _rotationSpeed;
    protected float _health;
    protected int _worth;

    public EnemyData _selfData;
    
    protected Collider _col;
    protected Outline _outline;

    protected List<Transform> _path;

    public bool IsAlive {  get; protected set; }

    public void Init(float maxHealth, float rotationSpeed, Transform[] path)
    {
        _path = new List<Transform>(path);

        _maxHealth = maxHealth;
        _moveSpeed = _selfData.speed;
        _rotationSpeed = rotationSpeed;

        _health = _maxHealth;
        _worth = 10;

        _outline.enabled = false;
        _col.enabled = true;
        IsAlive = true;
        Respawn();
    }

    public abstract void Hit(float damage);

    public abstract void Die();

    public abstract void Despawn();

    protected abstract void Respawn();

    public abstract void SetOutlineEnable(bool enable);

    public float GetHealthPercentage()
    {
        return _health / _maxHealth;
    }
}
