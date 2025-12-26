using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [Header("Tower Prefab")]
    [SerializeField] private GameObject towerPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private int startingLevel = 1;

    [Header("Default Build")]
    [SerializeField] private TowerArchetype defaultArchetype = TowerArchetype.AntiVirus;

    // Call from UI - basic spawn
    public void SpawnNewTower()
    {
        SpawnTowerWithBuild(defaultArchetype, startingLevel);
    }

    // Call from UI with specific build
    public void SpawnTowerWithBuild(TowerArchetype archetype, int level = 1)
    {
        if (towerPrefab == null)
        {
            Debug.LogError("Tower prefab not assigned!");
            return;
        }

        GameObject newTower = Instantiate(towerPrefab, spawnPosition, Quaternion.identity, transform);

        TowerStats stats = newTower.GetComponent<TowerStats>();
        if (stats != null)
        {
            stats.Initialize(archetype, level);
            Debug.Log(stats.GetStatsSummary());
        }
        else
        {
            Debug.LogError("Tower prefab missing TowerStats component!");
        }
    }
}
