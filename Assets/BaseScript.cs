using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseScript : MonoBehaviour
{
    public float placementRadius = 0.1f; 
    public GameObject raidusIndicator;
    public Material terrainMaterial;
    public GameObject building;

    public static int credits = 3000;
    public static event Action<int> CreditsChanged;

    public static bool HasCredits(int amount)
    {
        return credits >= amount;
    }

    public static bool TrySpendCredits(int amount)
    {
        if (!HasCredits(amount))
        {
            return false;
        }

        SetCredits(credits - amount);
        return true;
    }

    public static void AddCredits(int amount)
    {
        SetCredits(credits + amount);
    }

    private static void SetCredits(int amount)
    {
        credits = Mathf.Max(0, amount);
        CreditsChanged?.Invoke(credits);
    }

    // Method to increase the radius
    private void Update()
    {
        UpdateRadiusVisual();
    }
    public void IncreasePlacementRadius(float amount)
    {
        placementRadius += amount;
        print(placementRadius);
        UpdateRadiusVisual();

    }

    

    private void UpdateRadiusVisual()
    {
         if (raidusIndicator == null || terrainMaterial == null)
         {
             return;
         }

         raidusIndicator.transform.localScale = new Vector3(placementRadius, placementRadius / 100, placementRadius);
         terrainMaterial.SetFloat("_Radius", placementRadius/5);
    }

    private void OnDrawGizmos()
    {
        if (building == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(building.transform.position, placementRadius * 100);
    }

    public void AddGeneratorRadius()
    {
        placementRadius += 0.01f;
        //UpdateRadiusVisual();
        Debug.Log("Generator placed. New radius: " + placementRadius);
    }

    
    public void RemoveGeneratorRadius()
    {
        placementRadius -= 0.3f;
        UpdateRadiusVisual();
        Debug.Log("Generator destroyed. New radius: " + placementRadius);
    }
}
