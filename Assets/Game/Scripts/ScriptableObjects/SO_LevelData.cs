#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using UnityEngine;

namespace Rush.Game
{
    [CreateAssetMenu(fileName = "SO_LevelData", menuName = "Scriptable Objects/LevelData")]
    public class SO_LevelData : ScriptableObject
    {
        [SerializeField] public GameObject levelPrefab;
        [SerializeField] public string levelName;
        [SerializeField] public int stopperTicks;
        [SerializeField] public int cubesPerSpawner;
        [SerializeField] private int _InventoryArrows, _InventoryConvoyer, _InventoryDispatcher, _InventoryStopper;


        public Dictionary<Tile.TileVariants, int> tileInventory { get; private set; }


            void Awake()
            {
                tileInventory = new Dictionary<Tile.TileVariants, int>()
                {
                    { Tile.TileVariants.Arrow, _InventoryArrows },
                    { Tile.TileVariants.Convoyer, _InventoryConvoyer },
                    { Tile.TileVariants.Dispatcher, _InventoryDispatcher },
                    { Tile.TileVariants.Stopper, _InventoryStopper }
                };
            }
    }
}