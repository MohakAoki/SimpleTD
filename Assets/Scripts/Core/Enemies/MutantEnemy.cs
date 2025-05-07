using System.Collections;
using UnityEngine;

public class MutantEnemy : Enemy
{
    [SerializeField] private bool _walk;
    [SerializeField] private Animator[] _anims;

    private int _nextPoint;

    private void Awake()
    {
        _selfCollider = GetComponent<Collider>();
        _outline = GetComponent<Outline>();
    }

    public override void Hit(float damage)
    {
        if (!IsAlive)
            return;

        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    public override void Heal(float amount)
    {
        _health = Mathf.Clamp(_health + amount, 0, _maxHealth);
    }

    public override void Die()
    {
        _selfCollider.enabled = false;
        IsAlive = false;
        _nextPoint = 0;
        GameManager.Instance.Money += _worth;
        StartCoroutine(DieAnimation());

    }

    public override void Despawn()
    {
        _selfCollider.enabled = false;
        IsAlive = false;
        EnemySpawner.Instance.DespawnEnemy(this);
    }

    public override void SetOutlineEnable(bool enable)
    {
        _outline.enabled = enable;
    }

    protected override void Respawn()
    {
        foreach (var anim in _anims)
        {
            anim.SetBool("Crouch", !_walk);
        }
    }

    private void Update()
    {
        if (IsAlive)
        {
            Move();
        }
    }

    private void Move()
    {
        foreach (Animator anim in _anims)
        {
            anim.SetFloat("Speed", _moveSpeed);
        }

        Vector3 dir = _path[_nextPoint].position - transform.position;

        if (dir.sqrMagnitude <= 1)
        {
            NextPoint();
        }
        else
        {
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
        }
        LookAtDir(dir);
    }

    private void LookAtDir(Vector3 dir)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotationSpeed);
    }

    private void NextPoint()
    {
        _nextPoint++;
        if (_nextPoint >= _path.Count)
        {
            _nextPoint = 0;
            //Todo: End of path
            Despawn();
        }
    }

    private IEnumerator DieAnimation()
    {
        foreach (Animator anim in _anims)
        {
            anim.SetTrigger("Die");
        }
        yield return new WaitForSeconds(_anims[0].GetCurrentAnimatorClipInfo(0).Length);
        Despawn();
    }

}
