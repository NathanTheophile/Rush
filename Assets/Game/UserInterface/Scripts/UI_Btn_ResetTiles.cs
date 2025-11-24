using Rush.Game;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Btn_ResetTiles : MonoBehaviour
{
    [SerializeField] private Button _Button;
    [SerializeField] private TilePlacer _TilePlacer;
    [SerializeField] private UI_Inventory _Inventory;
    [SerializeField] private Manager_Game.GameStates _RequiredState = Manager_Game.GameStates.Setup;

    void Start()
    {
        if (_Button == null)
            _Button = GetComponent<Button>();

        _Button.onClick.AddListener(ResetTiles);
    }

    private void ResetTiles()
    {
        if (Manager_Game.Instance != null && Manager_Game.Instance.CurrentState != _RequiredState)
            return;

        _TilePlacer?.ResetPlacedTiles();
        _Inventory?.ResetInventory();
    }
}