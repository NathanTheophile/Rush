#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Rush.Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Btn_Navigation : MonoBehaviour
{
    #region _____________________________/ BTN TYPES
    public enum BtnTransitions { Show, Hide, Switch, Quit }

    [Header("Type")][SerializeField] private BtnTransitions BtnType;

    #endregion

    #region _____________________________/ REFS
    [Header("Optionnal References")]
    [SerializeField] private Button _Button;
    [SerializeField] private Transform _PanelToShow;
    [SerializeField] private Transform _PanelToHide;

    [SerializeField] private Manager_Game.GameStates _TargetState = Manager_Game.GameStates.Cards;

    #endregion

    #region _____________________________| INIT

    void Start()
    {
        InitMissingReferences();

        _Button.onClick.AddListener(delegate { Manager_Game.Instance.SetState(_TargetState); });

        switch (BtnType)
        {
            case BtnTransitions.Show:   _Button.onClick.AddListener(Show);      break;
            case BtnTransitions.Hide:   _Button.onClick.AddListener(Hide);      break;
            case BtnTransitions.Switch: _Button.onClick.AddListener(Switch);    break;
            case BtnTransitions.Quit:   _Button.onClick.AddListener(QuitGame);  break;
            default: break;
        }
        _Button = GetComponent<Button>();
    }

    private void InitMissingReferences()
    {
        if (_Button == null) _Button = GetComponent<Button>();
        if (_PanelToShow == null) _PanelToShow = transform.parent;
        if (_PanelToHide == null) _PanelToHide = transform.parent;
    }

    #endregion

    #region _____________________________| UI METHODS
   
    private void Show() => Instantiate(_PanelToShow, Vector3.zero, Quaternion.identity);

    private void Hide() => Destroy(_PanelToHide.GameObject());

    private void Switch() {
        Instantiate(_PanelToShow, transform.root);
        Destroy(_PanelToHide.GameObject()); }

    void QuitGame() => Application.Quit();

    #endregion
}