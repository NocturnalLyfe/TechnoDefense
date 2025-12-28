using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [Header("Tower Prefab")]
    [SerializeField] private GameObject towerPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private int startingLevel = 1;

    public void SpawnAntiVirus()
    {
        SpawnTowerWithBuild(TowerArchetype.AntiVirus);
    }

    public void SpawnAntiSpyware()
    {
        SpawnTowerWithBuild(TowerArchetype.AntiSpyware);
    }
    public void SpawnFirewall()
    {
        SpawnTowerWithBuild(TowerArchetype.Firewall);
    }

    // Call from UI with specific build
    public void SpawnTowerWithBuild(TowerArchetype archetype)
    {
        if (towerPrefab == null)
        {
            Debug.LogError("Tower prefab not assigned!");
            return;
        }

        GameObject newTower = Instantiate(towerPrefab, spawnPosition, Quaternion.identity, transform);

        TowerInstance instance = newTower.GetComponent<TowerInstance>();
        if (instance != null)
        {
            instance.Initialize(archetype, startingLevel);
            Debug.Log(instance.GetStatsSummary());
        }
        else
        {
            Debug.LogError("Tower prefab missing TowerStats component!");
        }
    }
}
