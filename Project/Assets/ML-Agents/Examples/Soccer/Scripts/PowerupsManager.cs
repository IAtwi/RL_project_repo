using UnityEngine;

public class PowerupsManager : MonoBehaviour
{
    //public GameObject spherePrefab;
    //public float spawnInterval = 5f;
    //private float timer = 0f;

    void Start()
    {
        // Create a default golden sphere prefab if not assigned
        //if (spherePrefab == null)
        //{
        //    spherePrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    spherePrefab.transform.localScale = Vector3.one * 1.5f; // Slightly bigger sphere
        //    Renderer renderer = spherePrefab.GetComponent<Renderer>();
        //    renderer.material = new Material(Shader.Find("Standard"));
        //    renderer.material.color = new Color(1f, 0.84f, 0f); // Gold color
        //}
    }

    void Update()
    {
        //timer += Time.deltaTime;

        //if (timer >= spawnInterval)
        //{
        //    SpawnSphere();
        //    timer = 0f; // Reset the timer
        //}
    }

    void SpawnSphere()
    {
        // Generate a random position in the specified range
        //float randomX = Random.Range(-6f, 6f);
        //float randomZ = Random.Range(-7f, 7f);
        //Vector3 spawnPosition = new Vector3(randomX, 0.5f, randomZ); // Spawning slightly above ground

        //// Instantiate the sphere
        //Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
    }
}
