using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool IsRoot;

    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _mountTransform;

    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform[] _shootPoints;

    private float _range;
    private float _recoil;
    private float _damage;
    private float _recoilTimer;
    private float _rotationSpeed;
    private int _currentShootPoint;

    private Enemy _target;
    private Outline _outline;

    private int _rangeUpgrade;
    private int _recoilUpgrade;
    private int _speedUpgrade;

    public TowerData SelfData { get; private set; }

    public bool IsActive { get; private set; }

    public void Init(TowerData data)
    {
        SelfData = data;

        _damage = data.damage;
        _range = data.rangeUpgrades[_rangeUpgrade].range;
        _recoil = data.recoilUpgrades[_recoilUpgrade].recoilSpeed;
        _rotationSpeed = data.rotationUpgrades[_speedUpgrade].rotationSpeed;

        IsActive = true;
    }

    public void Deactive()
    {
        IsActive = false;
    }

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _range, InputManager.Instance.EnemyLayer);

        float minDistance = float.PositiveInfinity;
        Collider targetCol = null;

        foreach (Collider collider in colliders)
        {
            float distanceToThis = Vector3.Distance(transform.position, collider.transform.position);
            if (distanceToThis < minDistance && collider.GetComponent<Enemy>().IsAlive)
            {
                minDistance = distanceToThis;
                targetCol = collider;
            }
        }

        if (targetCol != null)
        {
            _target = targetCol.GetComponent<Enemy>();
        }
        else
        {
            _target = null;
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            _recoilTimer += Time.deltaTime;

            float angle = FollowTarget();

            if (Mathf.Abs(angle) <= 15)
            {
                ShootTarget();
            }
        }
    }

    private float FollowTarget()
    {
        float angle = 180;

        if (_target != null)
        {
            Vector3 dir = _target.transform.position - _headTransform.position;
            Vector3 mountDir = _target.transform.position - _mountTransform.position;
            mountDir.y = 0;

            _mountTransform.rotation = Quaternion.Lerp(_mountTransform.rotation, Quaternion.LookRotation(mountDir), _rotationSpeed * Time.deltaTime);
            _headTransform.rotation = Quaternion.Lerp(_headTransform.rotation, Quaternion.LookRotation(dir), _rotationSpeed * Time.deltaTime);
            Vector3 euler = new Vector3(_headTransform.eulerAngles.x, _mountTransform.eulerAngles.y, _mountTransform.eulerAngles.z);
            _headTransform.eulerAngles = euler;
            angle = Vector3.SignedAngle(_headTransform.forward, dir, Vector3.up);
        }

        return angle;
    }

    private void ShootTarget()
    {
        if (_recoilTimer >= _recoil)
        {
            _recoilTimer = 0;
            int index = _currentShootPoint++ % _shootPoints.Length;
            Vector3 origin = _shootPoints[index].position;
            Vector3 dir = _target.transform.position - origin;

            ProjectileManager.Instance.ShootBullet(_bulletPrefab, origin, dir, 50, _damage);
        }
    }

    public void SetOutlineEnable(bool enabled)
    {
        _outline.enabled = enabled;
    }

    public float GetRange()
    {
        return _range;
    }

    public float GetRotationSpeed()
    {
        return _rotationSpeed;
    }

    public float GetDPS()
    {
        return _damage / _recoil;
    }

    public float GetNextRange()
    {
        if (_rangeUpgrade + 1 >= SelfData.rangeUpgrades.Length)
        {
            return 0;
        }
        return SelfData.rangeUpgrades[_rangeUpgrade + 1].range;
    }

    public float GetNextRotationSpeed()
    {
        if (_speedUpgrade + 1 >= SelfData.rotationUpgrades.Length)
        {
            return 0;
        }
        return SelfData.rotationUpgrades[_speedUpgrade + 1].rotationSpeed;
    }

    public float GetNextDPS()
    {
        if (_recoilUpgrade + 1 >= SelfData.recoilUpgrades.Length)
        {
            return 0;
        }
        return _damage / SelfData.recoilUpgrades[_recoilUpgrade + 1].recoilSpeed;
    }

    public int GetNextRangeCost()
    {
        if (_rangeUpgrade + 1 >= SelfData.rangeUpgrades.Length)
        {
            return 0;
        }
        return SelfData.rangeUpgrades[_rangeUpgrade + 1].upgradeCost;
    }

    public int GetNextRotationSpeedCost()
    {
        if (_speedUpgrade + 1 >= SelfData.rotationUpgrades.Length)
        {
            return 0;
        }
        return SelfData.rotationUpgrades[_speedUpgrade + 1].upgradeCost;
    }

    public int GetNextDPSCost()
    {
        if (_recoilUpgrade + 1 >= SelfData.recoilUpgrades.Length)
        {
            return 0;
        }
        return SelfData.recoilUpgrades[_recoilUpgrade + 1].upgradeCost;
    }

    public void UpgradeBase()
    {
        int cost = GetNextRangeCost();
        if (GameManager.Instance.Money >= cost)
        {
            GameManager.Instance.Money -= cost;
            _rangeUpgrade++;
            Init(SelfData);
        }
    }

    public void UpgradeMount()
    {
        int cost = GetNextRotationSpeedCost();
        if (GameManager.Instance.Money >= cost)
        {
            GameManager.Instance.Money -= cost;
            _speedUpgrade++;
            Init(SelfData);
        }
    }

    public void UpgradeHead()
    {
        int cost = GetNextDPSCost();
        if (GameManager.Instance.Money >= cost)
        {
            GameManager.Instance.Money -= cost;
            _recoilUpgrade++;
            Init(SelfData);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
