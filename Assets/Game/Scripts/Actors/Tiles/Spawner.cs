#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Rush.Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Rush.Game
{
    public class Spawner : Tile
    {
        [SerializeField] private Cube cubePrefab;
        [SerializeField] private SO_Colors _ColorSO;   

        Manager_Time timeManager;
        Manager_Tile tileManager;

        private Color _Color;

        private int _TickBetweenSpawns = 2;
        [SerializeField] private int _AmountoOfCubes = 1;
        private int _CurrentCubeSpawned = 0;

        void Awake()
        {
            _Color = _ColorSO.Color;
            GetComponentInChildren<Renderer>().material.color = _Color;
        }

        protected override void Start()
        {
            base.Start();
            timeManager = Manager_Time.Instance;
            tileManager = Manager_Tile.Instance;
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
            _CurrentCubeSpawned++;
            if (_CurrentCubeSpawned >= _AmountoOfCubes) timeManager.onTickFinished -= SpawnCube;
        }
    }
}
