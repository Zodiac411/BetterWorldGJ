using System;
using UnityEngine;

public class EconomyService : MonoBehaviour
{
    public static EconomyService Instance { get; private set; }

    [SerializeField] private int startingCredits = 3000;

    public event Action<int> CreditsChanged;

    public int Credits { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetCredits(startingCredits);
        BaseScript.BindPendingCreditsHandlers();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public bool HasCredits(int amount)
    {
        return Credits >= amount;
    }

    public bool TrySpendCredits(int amount)
    {
        if (!HasCredits(amount))
        {
            return false;
        }

        SetCredits(Credits - amount);
        return true;
    }

    public void AddCredits(int amount)
    {
        SetCredits(Credits + amount);
    }

    private void SetCredits(int amount)
    {
        Credits = Mathf.Max(0, amount);
        CreditsChanged?.Invoke(Credits);
    }
}
