using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private LayerMask _rayLayer;
    [SerializeField] private Transform _nodesParent;
    [SerializeField] private TowerData[] _availableTowers;
    [SerializeField] private Transform _radiusSphere;
    [SerializeField] private Rect _cameraBound;
    [SerializeField] private bool _usePan;

    public LayerMask EnemyLayer;

    private Tower _selectedTower;
    private Enemy _selectedEnemy;

    public bool IsRunning { get; private set; }

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void Init()
    {
        CameraSystem.Instance.SetBound(_cameraBound);
        CameraSystem.Instance.CanPan = _usePan;
        IsRunning = true;
    }

    private void Update()
    {
        if (!IsRunning)
            return;

        RayDetector();
    }

    private void RayDetector()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = CameraSystem.Instance.Ray();
            Debug.DrawRay(ray.origin, ray.direction * 500, Color.red, 1);

            if (Physics.Raycast(ray, out RaycastHit hit, 500, _rayLayer))
            {
                if (hit.collider.TryGetComponent(out Enemy enemy))
                {
                    SelectTower(null);
                    SelectEnemy(enemy);
                }
                else if (hit.collider.TryGetComponent(out Tower tower))
                {
                    SelectEnemy(null);

                    SelectTower(tower);
                }
                else
                {
                    SelectTower(null);
                    SelectEnemy(null);
                }
            }
        }
    }

    public void InstallTower(TowerData towerData)
    {
        if (_selectedTower == null)
        {
            Debug.LogWarning("No Tower Selected");
            return;
        }

        if (GameManager.Instance.Money < towerData.unlockCost)
        {
            return;
        }
        GameManager.Instance.Money -= towerData.unlockCost;

        Vector3 pos = _selectedTower.transform.position;

        GlobalPool.Instance.Pool(_selectedTower);
        _selectedTower.Deactive();

        Tower newNode = Instantiate(towerData.prefab, pos, Quaternion.identity, _nodesParent);
        newNode.Init(towerData);
        SelectTower(null);
    }

    public void SelectTower(Tower selected)
    {
        if (_selectedTower != null)
        {
            _selectedTower.SetOutlineEnable(false);
        }

        if (selected != null)
        {
            selected.SetOutlineEnable(true);
            float range = selected.GetRange() * 2;
            _radiusSphere.transform.localScale = new Vector3(range, .1f, range);
            _radiusSphere.transform.position = selected.transform.position - Vector3.down * .1f;
            _radiusSphere.gameObject.SetActive(true);

            if (selected.IsRoot)
            {
                InstallForm overlayForm = UI.Instance.GetForm<InstallForm>();
                overlayForm.SetFormData(_availableTowers.ToArray());
                UI.Instance.OpenForm(overlayForm);
            }
            else
            {
                UpgradeForm upgradeForm = UI.Instance.GetForm<UpgradeForm>();
                upgradeForm.SetData(selected.SelfData, selected);
                UI.Instance.OpenForm(upgradeForm);
            }
        }
        else
        {
            _radiusSphere.gameObject.SetActive(false);
            UI.Instance.CloseForm<InstallForm>();
        }
        _selectedTower = selected;
    }

    private void SelectEnemy(Enemy enemy)
    {
        if (_selectedEnemy != null)
        {
            _selectedEnemy.SetOutlineEnable(false);
        }

        if (enemy != null)
        {
            _selectedEnemy = enemy;
            enemy.SetOutlineEnable(true);
            MainForm mainForm = UI.Instance.GetForm<MainForm>();
            mainForm.SetEnemyPanelData(enemy);
            mainForm.UpdateEnemyHealth(enemy.GetHealthPercentage());
            mainForm.SetEnemyPanelVisibility(true);
        }
        else
        {
            UI.Instance.GetForm<MainForm>().SetEnemyPanelVisibility(false);
        }
    }

}
