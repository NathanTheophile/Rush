#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Rush.Game;
using UnityEngine;

namespace Rush.Game
{
    public class Spawner : Tile
    {
        [SerializeField] private Cube cubePrefab;
        [SerializeField] private SOColors _ColorSO;   

        TimeManager timeManager;
        TileManager tileManager;

        private Color _Color;

        private int _TickBetweenSpawns = 2;

        void Awake()
        {
            _Color = _ColorSO.Color;
            GetComponentInChildren<Renderer>().material.color = _Color;
        }

        protected override void Start()
        {
            base.Start();
            timeManager = TimeManager.Instance;
            tileManager = TileManager.Instance;
            timeManager.onTickFinished += SpawnCube;
            SpawnCube(0);
        }

        void SpawnCube(int pTickIndex)
        {
            if (pTickIndex % _TickBetweenSpawns != 0) return;
            Cube lCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
            lCube.SetColor(_Color);
            timeManager.objectsAffectedByTime.Add(lCube);
            timeManager.onTickFinished += lCube.TickUpdate;
            lCube.onTileDetected += tileManager.TryGetTile;
            lCube.SpawnDirection(direction);

        }
    }
}
