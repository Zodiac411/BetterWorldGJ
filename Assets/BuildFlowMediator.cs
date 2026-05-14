using UnityEngine;

public class BuildFlowMediator
{
    private readonly GameObject buildUI;
    private readonly GameObject generatorHologramPrefab;
    private readonly GameObject attackerHologramPrefab;
    private readonly TowerLogic towerLogic;
    private readonly IBuildPlacementFactory buildPlacementFactory;
    private const int generatorCost = 150;
    private const int attackerCost = 200;

    private GameObject currentHologram;

    public BuildFlowMediator(
        GameObject buildUI,
        GameObject generatorHologramPrefab,
        GameObject attackerHologramPrefab,
        TowerLogic towerLogic,
        IBuildPlacementFactory buildPlacementFactory = null)
    {
        this.buildUI = buildUI;
        this.generatorHologramPrefab = generatorHologramPrefab;
        this.attackerHologramPrefab = attackerHologramPrefab;
        this.towerLogic = towerLogic;
        this.buildPlacementFactory = buildPlacementFactory ?? new DefaultBuildPlacementFactory();

        if (this.buildUI != null)
        {
            this.buildUI.SetActive(false);
        }
    }

    public bool IsBuildModeActive
    {
        get { return buildUI != null && buildUI.activeSelf; }
    }

    public void ToggleBuildMode()
    {
        if (currentHologram != null)
        {
            Object.Destroy(currentHologram);
            currentHologram = null;
        }

        if (buildUI != null)
        {
            buildUI.SetActive(!buildUI.activeSelf);
        }
    }

    public void UpdatePreviewPosition(Vector3 position)
    {
        if (currentHologram != null)
        {
            currentHologram.transform.position = position;
        }
    }

    public bool TryStartBuilding(GameObject hologramPrefab, int cost)
    {
        if (hologramPrefab == null)
        {
            return false;
        }

        if (!BaseScript.HasCredits(cost))
        {
            Debug.Log("Not enough credits to build this turret.");
            return false;
        }

        if (currentHologram != null)
        {
            Object.Destroy(currentHologram);
        }

        currentHologram = buildPlacementFactory.CreateHologram(hologramPrefab);
        if (currentHologram == null)
        {
            return false;
        }

        Hologram hologram = currentHologram.GetComponent<Hologram>();
        if (hologram == null)
        {
            Object.Destroy(currentHologram);
            currentHologram = null;
            return false;
        }

        hologram.SetCost(cost);
        return true;
    }

    public bool TryPlaceTurret()
    {
        if (currentHologram == null)
        {
            return false;
        }

        if (towerLogic == null)
        {
            return false;
        }

        if (!towerLogic.CanPlaceTower(currentHologram.transform.position))
        {
            Debug.Log("Can't place tower here!");
            return false;
        }

        Hologram hologramScript = currentHologram.GetComponent<Hologram>();
        if (hologramScript == null)
        {
            return false;
        }

        if (!BaseScript.TrySpendCredits(hologramScript.Cost))
        {
            Debug.Log("Not enough credits to place this turret.");
            return false;
        }

        buildPlacementFactory.PlaceTower(hologramScript.turretPrefab, currentHologram.transform.position, Quaternion.identity);
        Debug.Log($"Turret placed. Credits left: {BaseScript.credits}");
        Object.Destroy(currentHologram);
        currentHologram = null;
        return true;
    }

    public bool TryStartGeneratorPlacement()
    {
        return TryStartBuilding(generatorHologramPrefab, generatorCost);
    }

    public bool TryStartAttackerPlacement()
    {
        return TryStartBuilding(attackerHologramPrefab, attackerCost);
    }
}
