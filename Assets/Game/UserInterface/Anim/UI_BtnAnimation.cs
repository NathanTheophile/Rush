using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BtnAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button self;
    [SerializeField] private Image _LeftHover, _RightHover;

    Vector2 maxScale = new Vector2(4f, 4f);
    float animSpeed = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void PlayEnterAnim()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, maxScale, Time.deltaTime * animSpeed);
        _LeftHover.GameObject().SetActive(true);
        _RightHover.GameObject().SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayEnterAnim();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
                transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, Time.deltaTime * animSpeed);
        _LeftHover.GameObject().SetActive(false);
        _RightHover.GameObject().SetActive(false);
    }
}
