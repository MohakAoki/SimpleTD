using UnityEngine;
using UnityEngine.UI;

public class InstallForm : Form
{
    [SerializeField] private TurrentOverview _overviewPrefab;
    [SerializeField] private RectTransform _contentParent;

    public override void Init()
    {

    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < _contentParent.childCount; i++)
        {
            _contentParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetFormData(params TowerData[] towers)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            TurrentOverview overview = null;

            if (i >= _contentParent.childCount)
            {
                overview = Instantiate(_overviewPrefab, _contentParent);
                overview.transform.localScale = Vector3.one;
            }
            else
            {
                overview = _contentParent.GetChild(i).GetComponent<TurrentOverview>();
                overview.gameObject.SetActive(true);
            }

            overview.SetData(towers[i]);
        }
    }
}
