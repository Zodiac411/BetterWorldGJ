using UnityEngine;

public class Hologram : MonoBehaviour
{
    [SerializeField] private TurretData turretDefinition;
    public GameObject turretPrefab;
    public int Cost { get; private set; }

    public TurretData Definition => turretDefinition;

    public void SetDefinition(TurretData definition)
    {
        turretDefinition = definition;
        if (definition == null)
        {
            return;
        }

        if (definition.prefab != null)
        {
            turretPrefab = definition.prefab;
        }

        SetCost(definition.buildCost);
    }

    public void SetCost(int cost)
    {
        Cost = cost;
    }

    private void Update()
    {
        if (Camera.main == null)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
}
