using System.Collections;
using UnityEngine;

public class DadyEnemy : Enemy
{
    [SerializeField] private float _healRange;
    [SerializeField] private float _healPerSecond;
    [SerializeField] private GameObject _healEffect;
    [SerializeField] private Animator[] _anims;

    private int _nextPoint;

    private void Awake()
    {
        _outline = GetComponentInChildren<Outline>();
        _selfCollider = GetComponent<Collider>();
        _healEffect.transform.localScale = Vector3.one * _healRange;
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
        IsAlive = false;
        _healEffect.SetActive(false);
        _nextPoint = 0;
        GameManager.Instance.Money += _worth;
        StartCoroutine(DieAnimation());

    }

    public override void Despawn()
    {
        _healEffect.SetActive(false);
        IsAlive = false;
        EnemySpawner.Instance.DespawnEnemy(this);
    }

    public override void SetOutlineEnable(bool enable)
    {
        _outline.enabled = enable;
    }

    protected override void Respawn()
    {
        _healEffect.SetActive(true);
    }

    private void Update()
    {
        if (IsAlive)
        {
            Move();
            Special();
        }
    }

    private void Special()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _healRange, InputManager.Instance.EnemyLayer);

        foreach (Collider col in cols)
        {
            if (col != _selfCollider && col.TryGetComponent(out Enemy e))
            {
                e.Heal(_healPerSecond * Time.deltaTime);
            }
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _healRange);
    }
}
