using UnityEngine;
using TMPro;

public class BaseUIScript : MonoBehaviour
{
    public GameObject hologramGeneratorPrefab;
    public GameObject hologramAttackerPrefab;
    public TextMeshProUGUI creditsText;

    private GameObject currentHologram;
    private EconomyService economyService;

    private void Awake()
    {
        economyService = EconomyService.Instance;
        if (economyService == null)
        {
            economyService = FindObjectOfType<EconomyService>();
        }
    }

    private void OnEnable()
    {
        if (economyService != null)
        {
            economyService.CreditsChanged += HandleCreditsChanged;
            HandleCreditsChanged(economyService.Credits);
        }
        else
        {
            BaseScript.CreditsChanged += HandleCreditsChanged;
            HandleCreditsChanged(BaseScript.credits);
        }
    }

    private void OnDisable()
    {
        if (economyService != null)
        {
            economyService.CreditsChanged -= HandleCreditsChanged;
        }
        else
        {
            BaseScript.CreditsChanged -= HandleCreditsChanged;
        }
    }

    private void PurchaseTurret(GameObject turretPrefab, GameObject hologramPrefab, int cost, SlotHolder slot)
    {
        if (!BaseScript.HasCredits(cost))
        {
            return;
        }

        EnterPlacementMode(hologramPrefab);
    }

    private void HandleCreditsChanged(int credits)
    {
        if (creditsText != null)
        {
            creditsText.text = $"Credits: {credits}";
        }
    }

    private void EnterPlacementMode(GameObject hologramPrefab)
    {
        if (currentHologram != null)
        {
            Destroy(currentHologram);
        }

        currentHologram = Instantiate(hologramPrefab);
        currentHologram.transform.rotation = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f));
    }

    public void PlaceTurret()
    {
        if (currentHologram == null)
        {
            return;
        }

        Instantiate(
            currentHologram.GetComponent<Hologram>().turretPrefab,
            currentHologram.transform.position,
            currentHologram.transform.rotation);
        Destroy(currentHologram);
    }
}
