using UnityEngine;

public class TickManager : MonoBehaviour
{
    private static float _TickSpeed;

    public float TickSpeed
        { get => _TickSpeed; set { if ((value >= 0f) && (value <= 3f)) _TickSpeed = value; }}
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
