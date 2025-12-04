using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem _ThisExplosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ThisExplosion = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ThisExplosion.time >= _ThisExplosion.main.duration) Destroy(gameObject);
    }
}
