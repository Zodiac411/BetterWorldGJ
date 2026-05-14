    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class buildManager : MonoBehaviour
    {
    public GameObject buildUI;
    public GameObject generatorHologramPrefab;
    public GameObject attackerHologramPrefab;
    public TowerLogic towerLogic;
    private BuildFlowMediator buildFlowMediator;
    void Start()
    {
        towerLogic = GetComponent<TowerLogic>();
        buildFlowMediator = new BuildFlowMediator(
            buildUI,
            generatorHologramPrefab,
            attackerHologramPrefab,
            towerLogic,
            new DefaultBuildPlacementFactory());
    }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("Toggled build mode.");
                ToggleBuildMode();
            }

            if (buildFlowMediator.IsBuildModeActive)
            {
                Camera playerCamera = Camera.main;
                if (playerCamera == null)
                {
                    return;
                }

                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                {
                    buildFlowMediator.UpdatePreviewPosition(hit.point);

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Debug.Log("Q pressed for generator.");
                        buildFlowMediator.TryStartGeneratorPlacement();
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("E pressed for attacker.");
                        buildFlowMediator.TryStartAttackerPlacement();
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Attempting to place turret.");
                        PlaceTurret();
                    }
                }
                else
                {
                    Debug.Log("Raycast did not hit the ground layer.");
                }
            }
        }

    void ToggleBuildMode()
    {
        buildFlowMediator.ToggleBuildMode();
    }

    public void StartBuilding(GameObject hologramPrefab, int cost)
    {
        buildFlowMediator.TryStartBuilding(hologramPrefab, cost);
    }

    public void PlaceTurret()
    {
        buildFlowMediator.TryPlaceTurret();
    } 
}
