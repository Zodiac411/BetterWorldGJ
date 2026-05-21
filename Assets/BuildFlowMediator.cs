using UnityEngine;

public class BuildFlowMediator
{
    private readonly GameObject buildUI;
    private readonly TurretData generatorDefinition;
    private readonly TurretData attackerDefinition;
    private readonly TowerLogic towerLogic;
    private readonly IBuildPlacementFactory buildPlacementFactory;

    private GameObject currentHologram;

    public BuildFlowMediator(
        GameObject buildUI,
        TurretData generatorDefinition,
        TurretData attackerDefinition,
        TowerLogic towerLogic,
        IBuildPlacementFactory buildPlacementFactory = null)
    {
        this.buildUI = buildUI;
        this.generatorDefinition = generatorDefinition;
        this.attackerDefinition = attackerDefinition;
        this.towerLogic = towerLogic;
        this.buildPlacementFactory = buildPlacementFactory ?? new DefaultBuildPlacementFactory();

        if (this.buildUI != null)
        {
            this.buildUI.SetActive(false);
        }
    }

    public bool IsBuildModeActive => buildUI != null && buildUI.activeSelf;

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

    public void ProcessBuildInput(Camera playerCamera, Vector2 mousePosition, bool generatorPressed, bool attackerPressed, bool placePressed)
    {
        if (!IsBuildModeActive || playerCamera == null)
        {
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return;
        }

        UpdatePreviewPosition(hit.point);

        if (generatorPressed)
        {
            TryStartGeneratorPlacement();
        }
        else if (attackerPressed)
        {
            TryStartAttackerPlacement();
        }
        else if (placePressed)
        {
            TryPlaceTurret();
        }
    }

    public void UpdatePreviewPosition(Vector3 position)
    {
        if (currentHologram != null)
        {
            currentHologram.transform.position = position;
        }
    }

    public bool TryStartBuilding(TurretData definition)
    {
        if (definition == null || definition.hologramPrefab == null)
        {
            return false;
        }

        if (!BaseScript.HasCredits(definition.buildCost))
        {
            Debug.Log("Not enough credits to build this turret.");
            return false;
        }

        if (currentHologram != null)
        {
            Object.Destroy(currentHologram);
        }

        currentHologram = buildPlacementFactory.CreateHologram(definition.hologramPrefab);
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

        hologram.SetDefinition(definition);
        return true;
    }

    public bool TryPlaceTurret()
    {
        if (currentHologram == null || towerLogic == null)
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

        GameObject prefab = hologramScript.turretPrefab;
        if (prefab == null && hologramScript.Definition != null)
        {
            prefab = hologramScript.Definition.prefab;
        }

        buildPlacementFactory.PlaceTower(
            prefab,
            currentHologram.transform.position,
            Quaternion.identity);

        Debug.Log($"Turret placed. Credits left: {BaseScript.credits}");
        Object.Destroy(currentHologram);
        currentHologram = null;
        return true;
    }

    public bool TryStartGeneratorPlacement()
    {
        return TryStartBuilding(generatorDefinition);
    }

    public bool TryStartAttackerPlacement()
    {
        return TryStartBuilding(attackerDefinition);
    }
}
