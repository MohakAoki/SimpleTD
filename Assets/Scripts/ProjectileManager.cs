using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    List<Bullet> _activeBullets;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        Init();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Init()
    {
        _activeBullets = new List<Bullet>();
    }

    private void RegisterBullet(Bullet bullet)
    {
        _activeBullets.Add(bullet);
    }

    public void ShootBullet(Bullet bullet, Vector3 origin, Vector3 dir, float speed, float damage)
    {
        Bullet bu = GlobalPool.Instance.GetObject<Bullet>();
        if (bu == null)
        {
            bu = Instantiate(bullet, transform);
        }
        bu.Init(speed, damage, origin, dir);
        AudioManager.Instance.PlaySFXAt("sfx_shoot", 2, origin);

        RegisterBullet(bu);
    }

    public void OnBulletHit(Bullet bullet, Enemy target)
    {
        if (target != null)
            target.Hit(bullet.Damage);

        _activeBullets.Remove(bullet);
        GlobalPool.Instance.Pool(bullet);
    }

    public void ClearBullets()
    {
        _activeBullets.Clear();
    }

    private void Update()
    {
        foreach (Bullet bullet in _activeBullets)
        {
            float moveAmount = bullet.Speed * Time.deltaTime;
            Vector3 dir = bullet.transform.forward * moveAmount;

            bullet.transform.position += dir;
        }
    }
}
