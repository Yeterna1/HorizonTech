using UnityEngine;
using System.Collections.Generic; // For list management if needed

// Attach this script to an empty GameObject to manage grass placement
public class GrassGroup : MonoBehaviour
{
    [Header("Editor Settings")]
    [Tooltip("Enable to paint grass by clicking in Scene view")]
    public bool editorMode = false; // Toggle grass painting mode
    [Tooltip("Grass prefab to instantiate")]
    public GameObject grassPrefab = null; // Reference to grass prefab
    [Tooltip("Terrain where grass will be placed")]
    public Terrain terrain = null; // Target terrain reference

    [Header("Grass Properties")]
    [Tooltip("Randomize Y-axis rotation when placing grass")]
    public bool randomRotationY = true; // Random facing direction
    [Tooltip("Minimum random scale value")]
    public float minScale = 0.8f; // Lower bound for size variation
    [Tooltip("Maximum random scale value")]
    public float maxScale = 1.2f; // Upper bound for size variation

    // These variables are controlled by GrassGroup_Inspector editor script
    [HideInInspector] 
    public float radius = 1.0f; // Brush radius for placement
    [HideInInspector] 
    public int count = 5; // Number of grass instances per click

    // Automatically disable editor mode when game starts
    void Start()
    {
        editorMode = false;
    }

    /// <summary>
    /// Spawns a cluster of grass around the specified world position
    /// </summary>
    /// <param name="centerPosition">World position where grass should be placed</param>
    public void AddGrassNode(Vector3 centerPosition)
    {
        // --- Safety checks ---
        if (grassPrefab == null)
        {
            Debug.LogError("Missing grass prefab! Please assign in Inspector.");
            return;
        }
        if (terrain == null)
        {
            Debug.LogError("Missing terrain reference! Please assign in Inspector.");
            return;
        }

        // --- Grass instantiation ---
        for (int i = 0; i < count; i++)
        {
            // Create new grass instance
            GameObject go = Instantiate(grassPrefab);

            // Parent to this manager for organization
            go.transform.SetParent(transform);

            // Calculate random position within brush radius
            Vector2 randomOffset2D = Random.insideUnitCircle * radius;
            Vector3 randomPosition = centerPosition + new Vector3(randomOffset2D.x, 0, randomOffset2D.y);

            // Adjust position to terrain height
            randomPosition.y = terrain.SampleHeight(randomPosition);

            // Apply final position
            go.transform.position = randomPosition;

            // Random Y rotation if enabled
            if (randomRotationY)
            {
                go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            }

            // Apply random uniform scale
            float scale = Random.Range(minScale, maxScale);
            go.transform.localScale = Vector3.one * scale;

            // Optional naming for debugging
            go.name = $"{grassPrefab.name}_{transform.childCount}";
        }
    }
}