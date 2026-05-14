using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hologramGeneratorPrefab; 
    public GameObject hologramAttackerPrefab; 
    public TextMeshProUGUI creditsText;
    private GameObject currentHologram;

    private void OnEnable()
    {
        BaseScript.CreditsChanged += HandleCreditsChanged;
        HandleCreditsChanged(BaseScript.credits);
    }

    private void OnDisable()
    {
        BaseScript.CreditsChanged -= HandleCreditsChanged;
    }


    void PurchaseTurret(GameObject turretPrefab, GameObject hologramPrefab, int cost, SlotHolder slot)
    {
        if (BaseScript.TrySpendCredits(cost))
        {
            EnterPlacementMode(hologramPrefab);
        }
    }

    private void HandleCreditsChanged(int credits)
    {
        if (creditsText != null)
        {
            creditsText.text = $"Credits: {credits}";
        }
    }

    void EnterPlacementMode(GameObject hologramPrefab)
    {
        if (currentHologram != null)
            Destroy(currentHologram);

        currentHologram = Instantiate(hologramPrefab);
        // If your turrets need to be rotated to face a certain direction upon instantiation, adjust here.
        currentHologram.transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
    }

    public void PlaceTurret()
    {
        if (currentHologram != null)
        {
            // Instantiate the actual turret prefab at the hologram's position
            Instantiate(currentHologram.GetComponent<Hologram>().turretPrefab, currentHologram.transform.position, currentHologram.transform.rotation);
            Destroy(currentHologram); // Destroy the hologram
        }
    }
   
}
