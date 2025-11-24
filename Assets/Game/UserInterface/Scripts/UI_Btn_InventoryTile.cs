using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Btn_InventoryTile : MonoBehaviour
{
    [SerializeField] private TMP_Text _TxtTileName;
    [SerializeField] private TMP_Text _TxtTileAmount;
    [SerializeField] private int _TileAmount;

    public void UpdateTileAmount()
    {
        _TileAmount --;
        if (_TileAmount == 0) Destroy(gameObject);
        _TxtTileAmount.text = _TileAmount.ToString();
    }
}
