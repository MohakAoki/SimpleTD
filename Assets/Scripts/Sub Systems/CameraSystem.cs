using UnityEngine;
using static UnityEditor.PlayerSettings;

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

    public bool CanZoom;
    public bool CanPan;

    private Camera _cam;

    private float _defaultZoom;
    private Vector3 _zoomVelocity;
    private Vector3 _panVelocity;
    private Vector3 _totalVelocity;
    private Vector2 _lastPos;

    private float _currentDrag;

    public Ray Ray()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        return ray;
    }

    public void SetBound(Rect bound)
    {
        _boundRight.x = bound.x;
        _boundRight.y = bound.xMax;

        _boundUp.x = bound.y;
        _boundUp.y = bound.yMax;
    }


    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        _cam = GetComponent<Camera>();
        _currentDrag = _drag;
        _defaultZoom = _cam.transform.position.y;
    }

    private void OnDestroy()
    {
        Instance = null;
    }


    private void Update()
    {
        Pan();
        Zoom();

        ValidatePosition();
    }

    private void Pan()
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
                _panVelocity += new Vector3(dir.y, 0, -dir.x).normalized * _speed;
            }
        }
    }

    private void Zoom()
    {
        if (!CanZoom)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            _zoomVelocity += _cam.transform.forward * _speed;
        }
        else if (scroll < 0)
        {
            _zoomVelocity += -_cam.transform.forward * _speed;
        }
    }

    private void ValidatePosition()
    {
        _totalVelocity = _panVelocity + _zoomVelocity;
        if (_totalVelocity.magnitude > 0)
        {
            Vector3 pos = _cam.transform.position;
            pos += _totalVelocity * Time.deltaTime;
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

            if (pos.y <= _zoom.x)
            {
                pos.y = _zoom.x;
                _zoomVelocity = Vector3.zero;
            }
            else if (pos.y >= _zoom.y)
            {
                pos.y = _zoom.y;
                _zoomVelocity = Vector3.zero;
            }

            _cam.transform.position = pos;
            _panVelocity -= _panVelocity.normalized * _panVelocity.magnitude * _currentDrag;
            _zoomVelocity -= _zoomVelocity.normalized * _zoomVelocity.magnitude * _currentDrag;

            if (_totalVelocity.sqrMagnitude <= _movethreshold)
            {
                _totalVelocity = Vector3.zero;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Vector3 size = new Vector3(_boundRight.x + _boundRight.y, 25, _boundUp.x + _boundUp.y);
        Vector3 center = new Vector3(_boundRight.x, 0, _boundUp.x) + size / 2;
        Gizmos.DrawWireCube(center, size);
    }
}
