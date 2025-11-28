#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;
using UnityEngine.UI;
using Rush.Game.Core;

public class UI_Btn_Navigation : MonoBehaviour
{
    #region _____________________________/ BTN TYPES
    public enum BtnTransitions { Show, Hide, Switch, Quit }

    [Header("Type")][SerializeField] private BtnTransitions _BtnType;

    #endregion

    #region _____________________________/ REFS
    [Header("Optionnal References")]
    [SerializeField] private Button _Button;
    [SerializeField] private Transform _CardToShow;
    [SerializeField] private Transform _CardToHide;

    #endregion

    #region _____________________________| INIT

    void Start()
    {
        Init();

        if (_Button == null) return;

        switch (_BtnType)
        {
            case BtnTransitions.Show:   _Button.onClick.AddListener(Show);      break;
            case BtnTransitions.Hide:   _Button.onClick.AddListener(Hide);      break;
            case BtnTransitions.Switch: _Button.onClick.AddListener(Switch);    break;
            case BtnTransitions.Quit:   _Button.onClick.AddListener(QuitGame);  break;
            default: break;
        }
    }

    private void Init()
    {
        if (_Button == null) _Button = GetComponent<Button>();
        if (_CardToShow == null) Debug.Log("A button has missing card to show.");
        if (_CardToHide == null) _CardToHide = transform.parent;
    }

    #endregion

    #region _____________________________| UI METHODS
   
    private void Show()     => Manager_Ui.Instance?.Show(_CardToShow);

    private void Hide()     => Manager_Ui.Instance?.Hide(_CardToHide);

    private void Switch()   => Manager_Ui.Instance?.Switch(_CardToShow, _CardToHide);

    void QuitGame()         => Application.Quit();

    #endregion
}