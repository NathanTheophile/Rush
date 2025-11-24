using Rush.Game;
using UnityEngine;
using UnityEngine.UI;

public class Btn_Transition : MonoBehaviour
{
    [SerializeField] private Button _BtnTransition;
    [SerializeField] private Transform _PanelToShow;
    [SerializeField] private Transform _PanelToHide;
    [SerializeField] private bool _UpdateGameState = false;
    [SerializeField] private Manager_Game.GameStates _TargetState = Manager_Game.GameStates.Cards;

    private Manager_Ui manager_Ui;
    private Manager_Game manager_Game;

    void Start()
    {
        manager_Ui = Manager_Ui.Instance;
        manager_Game = Manager_Game.Instance;
        if (_PanelToHide == null) _PanelToHide = transform.parent;
        _BtnTransition = GetComponent<Button>();
        _BtnTransition.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (manager_Ui != null)
        {
            manager_Ui.SwitchPanel(_PanelToShow, _PanelToHide);
        }

        if (_UpdateGameState && manager_Game != null)
        {
            manager_Game.SetState(_TargetState);
        }
    }
}
