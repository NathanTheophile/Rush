using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Btn_InventoryTile : MonoBehaviour
{
    [SerializeField] private TMP_Text _TileName;
    [SerializeField] private TMP_Text _TileAmount;
}
