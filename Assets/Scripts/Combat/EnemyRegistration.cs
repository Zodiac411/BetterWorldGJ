using UnityEngine;

[DisallowMultipleComponent]
public class EnemyRegistration : MonoBehaviour
{
    private void OnEnable()
    {
        EnemyRegistry.Register(transform);
    }

    private void OnDisable()
    {
        EnemyRegistry.Unregister(transform);
    }
}
