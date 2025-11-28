using Rush.Game;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Btn_ResetTiles : MonoBehaviour
{
    [SerializeField] private Button _Button;
    [SerializeField] private TilePlacer _TilePlacer;
    [SerializeField] private UI_Inventory _Inventory;

    void Start()
    {
        if (_Button == null)
            _Button = GetComponent<Button>();

        _Button.onClick.AddListener(ResetTiles);
    }

    private void ResetTiles()
    {
        _TilePlacer?.ResetPlacedTiles();
        _Inventory?.ResetInventory();
    }
}