using UnityEngine;

public class Bullet : MonoBehaviour
{
    private TrailRenderer _trail;

    public LayerMask ignorLayer;
    public LayerMask targetLayer;

    public float Speed { get; private set; }
    public float Damage { get; private set; }

    private void Awake()
    {
        _trail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & ignorLayer) != 0)
            return;


        Enemy enemy = null;
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            enemy = other.GetComponent<Enemy>();
        }

        _trail.emitting = false;
        ProjectileManager.Instance.OnBulletHit(this, enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("GameController"))
        {
            _trail.emitting = false;
            ProjectileManager.Instance.OnBulletHit(this, null);
        }
    }

    public void Init(float speed, float damage, Vector3 pos, Vector3 dir)
    {
        Speed = speed;
        Damage = damage;
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(dir);
        _trail.emitting = true;
        gameObject.SetActive(true);
    }
}
