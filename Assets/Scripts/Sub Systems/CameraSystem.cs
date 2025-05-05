using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem Instance;

    private Camera _cam;

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

    public Ray Ray()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        return ray;
    }
}
