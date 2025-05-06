using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem Instance;

    [SerializeField] private float _speed;
    [SerializeField] private float _movethreshold;
    [SerializeField, Range(0.01f, 1f)] private float _drag;
    [SerializeField, Range(0.01f, 1f)] private float _stiffness;

    [SerializeField] private Vector2 _zoom;
    [SerializeField] private Vector2 _boundUp;
    [SerializeField] private Vector2 _boundRight;

    public bool CanPan;

    private Camera _cam;

    private Vector3 _velocity;
    private Vector2 _lastPos;

    private bool _isPaning;
    private float _currentDrag;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        _cam = GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }


    private void Update()
    {
        if (!CanPan)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _lastPos = Input.mousePosition;
            _currentDrag = _stiffness;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _currentDrag = _drag;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            Vector2 dir = pos - _lastPos;
            _lastPos = pos;

            if (dir.sqrMagnitude > _movethreshold)
            {
                _isPaning = true;

                _velocity = new Vector3(dir.y, 0, -dir.x).normalized * _speed;
            }
        }

        if (_isPaning)
        {
            Vector3 pos = _cam.transform.position;
            pos += _velocity * Time.deltaTime;
            if (pos.x <= _boundRight.x)
            {
                pos.x = _boundRight.x;
            }
            else if (pos.x >= _boundRight.y)
            {
                pos.x = _boundRight.y;
            }

            if (pos.z <= _boundUp.x)
            {
                pos.z = _boundUp.x;
            }
            else if (pos.z >= _boundUp.y)
            {
                pos.z = _boundUp.y;
            }

            _cam.transform.position = pos;
            _velocity -= _velocity.normalized * _velocity.magnitude * _currentDrag;

            if (_velocity.sqrMagnitude <= _movethreshold)
            {
                _isPaning = false;
            }
        }
    }

    public Ray Ray()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        return ray;
    }

    public void EnablePan()
    {
        CanPan = true;
    }

    public void SetBound(Rect bound)
    {
        _boundRight.x = bound.x;
        _boundRight.y = bound.xMax;

        _boundUp.x = bound.y;
        _boundUp.y = bound.yMax;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Vector3 size = new Vector3(_boundRight.x + _boundRight.y, 25, _boundUp.x + _boundUp.y);
        Vector3 center = new Vector3(_boundRight.x, 0, _boundUp.x) + size / 2;
        Gizmos.DrawWireCube(center, size);
    }
}
