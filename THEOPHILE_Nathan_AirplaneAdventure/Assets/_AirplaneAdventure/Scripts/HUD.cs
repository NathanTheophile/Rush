using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Airplane _Airplane;
    [SerializeField] Text _ScoreText;
    private int _Score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Airplane.OnCollectibleHit += UpdateScore;
    }

    void UpdateScore()
    {
        _Score ++;
        _ScoreText.text = _Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
