using UnityEngine;

public interface IBuildPlacementFactory
{
    GameObject CreateHologram(GameObject hologramPrefab);
    GameObject PlaceTower(GameObject towerPrefab, Vector3 position, Quaternion rotation);
}

public sealed class DefaultBuildPlacementFactory : IBuildPlacementFactory
{
    public GameObject CreateHologram(GameObject hologramPrefab)
    {
        return Object.Instantiate(hologramPrefab);
    }

    public GameObject PlaceTower(GameObject towerPrefab, Vector3 position, Quaternion rotation)
    {
        return Object.Instantiate(towerPrefab, position, rotation);
    }
}
