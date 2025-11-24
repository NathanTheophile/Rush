using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Btn_InventoryTile : MonoBehaviour
{
    [SerializeField] public TMP_Text _TileName;
    [SerializeField] public TMP_Text _TileAmount;
}
