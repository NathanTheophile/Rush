using System.Collections.Generic;
using UnityEngine;

public class CollectiblesSpawner : MonoBehaviour
{
    [SerializeField] private Transform _CollectiblePrefab;
    [SerializeField] private GameObject _CollectibleExplosion;
    [SerializeField] private Airplane _Airplane;

    private List<Transform> _Spawners = new List<Transform>();

    Transform _CurrentCollectible;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform t in GetComponentInChildren<Transform>()) _Spawners.Add(t);
        
        _Airplane.OnCollectibleHit += DestroyCurrentCollectible; 

        SpawnCollectible();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCollectible()
    {
        int lSpawnerIndex = Random.Range(0, _Spawners.Count -1);

        _CurrentCollectible = Instantiate(_CollectiblePrefab, _Spawners[lSpawnerIndex]);
    }

    private void DestroyCurrentCollectible()
    {
        Instantiate(_CollectibleExplosion, _CurrentCollectible.position, Quaternion.identity);
        Destroy(_CurrentCollectible.gameObject);
        SpawnCollectible();
    }
}
