using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    [SerializeField] private EconomyService economyService;

    private static readonly List<Action<int>> PendingCreditsHandlers = new List<Action<int>>();

    public float placementRadius = 0.1f;
    public GameObject raidusIndicator;
    public Material terrainMaterial;
    public GameObject building;

    public static EconomyService Economy => EconomyService.Instance;

    public static int credits => Economy != null ? Economy.Credits : 0;

    public static event Action<int> CreditsChanged
    {
        add
        {
            if (Economy != null)
            {
                Economy.CreditsChanged += value;
            }
            else if (value != null && !PendingCreditsHandlers.Contains(value))
            {
                PendingCreditsHandlers.Add(value);
            }
        }
        remove
        {
            if (Economy != null)
            {
                Economy.CreditsChanged -= value;
            }

            PendingCreditsHandlers.Remove(value);
        }
    }

    public static bool HasCredits(int amount)
    {
        return Economy != null && Economy.HasCredits(amount);
    }

    public static bool TrySpendCredits(int amount)
    {
        return Economy != null && Economy.TrySpendCredits(amount);
    }

    public static void AddCredits(int amount)
    {
        Economy?.AddCredits(amount);
    }

    private void Awake()
    {
        if (economyService == null)
        {
            economyService = GetComponent<EconomyService>();
        }

        if (economyService == null)
        {
            economyService = gameObject.AddComponent<EconomyService>();
        }

        BindPendingCreditsHandlers();
    }

    public static void BindPendingCreditsHandlers()
    {
        if (Economy == null || PendingCreditsHandlers.Count == 0)
        {
            return;
        }

        for (int i = 0; i < PendingCreditsHandlers.Count; i++)
        {
            Economy.CreditsChanged += PendingCreditsHandlers[i];
        }

        PendingCreditsHandlers.Clear();
    }

    private void Update()
    {
        UpdateRadiusVisual();
    }

    public void IncreasePlacementRadius(float amount)
    {
        placementRadius += amount;
        UpdateRadiusVisual();
    }

    private void UpdateRadiusVisual()
    {
        if (raidusIndicator == null || terrainMaterial == null)
        {
            return;
        }

        raidusIndicator.transform.localScale = new Vector3(placementRadius, placementRadius / 100f, placementRadius);
        terrainMaterial.SetFloat("_Radius", placementRadius / 5f);
    }

    private void OnDrawGizmos()
    {
        if (building == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(building.transform.position, placementRadius * 100f);
    }

    public void AddGeneratorRadius()
    {
        placementRadius += 0.01f;
        Debug.Log("Generator placed. New radius: " + placementRadius);
    }

    public void RemoveGeneratorRadius()
    {
        placementRadius -= 0.3f;
        UpdateRadiusVisual();
        Debug.Log("Generator destroyed. New radius: " + placementRadius);
    }
}
