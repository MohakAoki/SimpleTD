using System.Collections;
using UnityEngine;

public class MutantEnemy : Enemy
{
    [SerializeField] private Animator[] _anims;

    private int _nextPoint;
    private Outline _outline;

    private void Awake()
    {
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

    public override void Die()
    {
        IsAlive = false;
        _nextPoint = 0;
        GameManager.Instance.Money += _worth;
        StartCoroutine(DieAnimation());

    }

    public override void Despawn()
    {
        IsAlive = false;
        EnemySpawner.Instance.DespawnEnemy(this);
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

    public override void SetOutlineEnable(bool enable)
    {
        _outline.enabled = enable;
    }
}
