using Rush.Game;
using UnityEngine;
using UnityEngine.UI;

public class Btn_Transition : MonoBehaviour
{
    [SerializeField] private Button _BtnTransition;
    [SerializeField] private Transform _PanelToShow;
    [SerializeField] private Transform _PanelToHide;

    private Manager_Ui manager_Ui;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager_Ui = Manager_Ui.Instance;
        if (_PanelToHide == null) _PanelToHide = transform.parent;
        _BtnTransition = GetComponent<Button>();
        _BtnTransition.onClick.AddListener(delegate { manager_Ui.SwitchPanel(_PanelToShow, _PanelToHide); });
    }
}
