using UnityEngine;
using UnityEngine.UI;

public class Btn_Quit : MonoBehaviour
{
    [SerializeField] private Button _BtnTransition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _BtnTransition = GetComponent<Button>();
        _BtnTransition.onClick.AddListener(QuitGame);
    }

    void QuitGame() => Application.Quit();

    // Update is called once per frame
    void Update()
    {
        
    }
}
