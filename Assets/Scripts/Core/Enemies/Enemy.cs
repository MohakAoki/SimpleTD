using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float _maxHealth;
    protected float _moveSpeed;
    protected float _rotationSpeed;
    protected float _health;
    protected int _worth;

    protected List<Transform> _path;

    public bool IsAlive {  get; protected set; }

    public void Init(float maxHealth, float moveSpeed, float rotationSpeed, Transform[] path)
    {
        _path = new List<Transform>(path);

        _maxHealth = maxHealth;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;

        _health = _maxHealth;
        _worth = 10;
        IsAlive = true;
    }

    public abstract void Hit(float damage);

    public abstract void Die();

    public abstract void Despawn();

    public abstract void SetOutlineEnable(bool enable);
}
