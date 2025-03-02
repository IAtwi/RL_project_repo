using UnityEngine;

public class PowerupsManager : MonoBehaviour
{
    public GameObject fieldGameObject;
    public GameObject powerUpPrefab;
    public float spawnInterval = 5f; // to be made private after fixing it

    private float _timer = 0f;
    private BoxCollider _fieldBoxCollider;

    #region Privates

    private void Start()
    {
        _fieldBoxCollider = fieldGameObject.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            SpawnSphere();
            _timer = 0f; // Reset the timer
        }
    }

    private void SpawnSphere()
    {
        Bounds bounds = _fieldBoxCollider.bounds;

        float fieldHalfWidth = bounds.size.z / 2;
        float fieldQuarterLength = bounds.size.x / 4;

        float randomX = Random.Range(-fieldQuarterLength, fieldQuarterLength);
        float randomZ = Random.Range(-fieldHalfWidth * 0.7f, fieldQuarterLength * 0.7f);
        Vector3 spawnPosition = new(bounds.center.x + randomX, powerUpPrefab.transform.localScale.y / 2, bounds.center.z + randomZ);

        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity, transform);
    }

    #endregion


    #region Publics

    public void Reset()
    {
        _timer = 0f;
        // reset all in effect power ups and destroy existing on field power ups
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    #endregion

}
