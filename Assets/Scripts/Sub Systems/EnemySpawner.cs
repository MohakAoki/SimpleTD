using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private float _waveCooldown;
    [SerializeField] private WaveData[] _waves;
    [SerializeField] private Transform _path;
    [SerializeField] private Transform _enemyParent;

    private int _totalEnemies;
    private int _killedEnemies;

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
        UI.Instance.GetForm<MainForm>().SetWavePanelVisibility(true);

        _totalEnemies = 0;
        _killedEnemies = 0;
        foreach (var wave in _waves)
        {
            _totalEnemies += wave.waveEntries.Count;
        }
        UI.Instance.GetForm<MainForm>().UpdateWave(current: 0, total: _totalEnemies);
        StartCoroutine(SpawnSystem());
    }

    private IEnumerator SpawnSystem()
    {      
        foreach (WaveData wave in _waves)
        {
            GameManager.Instance.Money += wave.reward;
            foreach (WaveEntry entry in wave.waveEntries)
            {
                yield return new WaitForSeconds(Random.Range(entry.spawnTime.x, entry.spawnTime.y));
                SpawnEnemy(entry, _path);
            }

            yield return new WaitForSeconds(_waveCooldown);
        }
    }

    private void SpawnEnemy(WaveEntry entry, Transform path)
    {
        GameObject obj = GlobalPool.Instance.GetObject(entry.Enemy._selfData.name);
        Enemy enemy = null;
        if (obj == null)
        {
            enemy = Instantiate(entry.Enemy, _enemyParent);
        }
        else
        {
            enemy = obj.GetComponent<Enemy>();
            enemy.transform.parent = _enemyParent;
        }

        Vector3 spawnPoint = path.GetChild(0).position;
        Vector3 lookDir = path.GetChild(1).position - spawnPoint;
        enemy.transform.position = spawnPoint;
        enemy.transform.rotation = Quaternion.LookRotation(lookDir);
        enemy.gameObject.SetActive(true);

        Transform[] movePath = new Transform[path.childCount - 1];
        for (int i = 0; i < movePath.Length; i++)
        {
            movePath[i] = path.GetChild(i + 1);
        }
        float health = Random.Range(entry.health.x, entry.health.y);
        enemy.Init(health, 2, movePath);
    }

    public void DespawnEnemy(Enemy enemy)
    {
        _killedEnemies++;
        UI.Instance.GetForm<MainForm>().UpdateWave(current: _killedEnemies, total: _totalEnemies);
        GlobalPool.Instance.Pool(enemy);
        if (_killedEnemies >= _totalEnemies)
        {
            LevelManager.Instance.OnLevelFinish();
        }
    }
}
