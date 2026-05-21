using System.Collections.Generic;
using UnityEngine;

public static class EnemyRegistry
{
    private static readonly List<Transform> ActiveEnemies = new List<Transform>();

    public static void Register(Transform enemy)
    {
        if (enemy == null || ActiveEnemies.Contains(enemy))
        {
            return;
        }

        ActiveEnemies.Add(enemy);
    }

    public static void Unregister(Transform enemy)
    {
        if (enemy == null)
        {
            return;
        }

        ActiveEnemies.Remove(enemy);
    }

    public static Transform FindNearest(Vector3 origin, float maxRange)
    {
        Transform nearest = null;
        float minDistance = maxRange;

        for (int i = ActiveEnemies.Count - 1; i >= 0; i--)
        {
            Transform enemy = ActiveEnemies[i];
            if (enemy == null)
            {
                ActiveEnemies.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(origin, enemy.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
