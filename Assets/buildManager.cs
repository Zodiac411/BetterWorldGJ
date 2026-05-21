using UnityEngine;

public class buildManager : MonoBehaviour
{
    public GameObject buildUI;
    [SerializeField] private TurretData generatorDefinition;
    [SerializeField] private TurretData attackerDefinition;
    [SerializeField] private GameObject generatorHologramPrefab;
    [SerializeField] private GameObject attackerHologramPrefab;
    public TowerLogic towerLogic;

    private BuildFlowMediator buildFlowMediator;

    private void Start()
    {
        if (towerLogic == null)
        {
            towerLogic = GetComponent<TowerLogic>();
        }

        EnsureLegacyHologramLinks();

        buildFlowMediator = new BuildFlowMediator(
            buildUI,
            generatorDefinition,
            attackerDefinition,
            towerLogic,
            new DefaultBuildPlacementFactory());
    }

    private void EnsureLegacyHologramLinks()
    {
        if (generatorDefinition != null && generatorDefinition.hologramPrefab == null)
        {
            generatorDefinition.hologramPrefab = generatorHologramPrefab;
        }

        if (attackerDefinition != null && attackerDefinition.hologramPrefab == null)
        {
            attackerDefinition.hologramPrefab = attackerHologramPrefab;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            buildFlowMediator.ToggleBuildMode();
        }

        if (!buildFlowMediator.IsBuildModeActive)
        {
            return;
        }

        buildFlowMediator.ProcessBuildInput(
            Camera.main,
            Input.mousePosition,
            Input.GetKeyDown(KeyCode.Q),
            Input.GetKeyDown(KeyCode.E),
            Input.GetMouseButtonDown(0));
    }

    public void StartBuilding(GameObject hologramPrefab, int cost)
    {
        TurretData definition = ResolveDefinitionForHologram(hologramPrefab, cost);
        if (definition != null)
        {
            buildFlowMediator.TryStartBuilding(definition);
        }
    }

    public void PlaceTurret()
    {
        buildFlowMediator.TryPlaceTurret();
    }

    private TurretData ResolveDefinitionForHologram(GameObject hologramPrefab, int cost)
    {
        if (generatorDefinition != null
            && (generatorDefinition.hologramPrefab == hologramPrefab || generatorHologramPrefab == hologramPrefab))
        {
            if (generatorDefinition.buildCost == 0 && cost > 0)
            {
                generatorDefinition.buildCost = cost;
            }

            return generatorDefinition;
        }

        if (attackerDefinition != null
            && (attackerDefinition.hologramPrefab == hologramPrefab || attackerHologramPrefab == hologramPrefab))
        {
            if (attackerDefinition.buildCost == 0 && cost > 0)
            {
                attackerDefinition.buildCost = cost;
            }

            return attackerDefinition;
        }

        return null;
    }
}
