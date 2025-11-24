using Rush.Game;
using UnityEngine;
using static Rush.Game.SO_LevelData;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] TilePlacer _TilePlacer;

    SO_LevelData _CurrentLevel;
    [SerializeField] Transform container;
    [SerializeField] UI_Btn_InventoryTile inventoryTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _CurrentLevel = Manager_Game.Instance.CurrentLevel;
        foreach (InventoryTile item in _CurrentLevel.inventory)
        {
            UI_Btn_InventoryTile lTile = Instantiate(inventoryTile, container);
            lTile.Initialize(item, _TilePlacer);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
