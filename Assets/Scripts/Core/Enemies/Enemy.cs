using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float _maxHealth;
    protected float _moveSpeed;
    protected float _rotationSpeed;
    protected float _health;
    protected int _worth; // Money on die

    public EnemyData _selfData;
    
    protected Collider _selfCollider;
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
        _worth = _selfData.worth;

        _outline.enabled = false;
        _selfCollider.enabled = true;
        IsAlive = true;
        Respawn();
    }

    public float GetHealthPercentage()
    {
        return _health / _maxHealth;
    }


    public abstract void Hit(float damage);

    public abstract void Heal(float amount);

    public abstract void Die();

    public abstract void Despawn();

    protected abstract void Respawn();

    public abstract void SetOutlineEnable(bool enable);

}
