#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Rush.Game.Core;
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
        Manager_Game gameManager;

        private Color _Color;

        private int _TickBetweenSpawns = 2;
        [SerializeField] private int _AmountoOfCubes = 1;
        private int _CurrentCubeSpawned = 0;
        private bool _Spawning = false;

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
            gameManager = Manager_Game.Instance;
            gameManager?.UpdateCubesAmountoComplete(_AmountoOfCubes);
            BeginSpawning();
        }

        void SpawnCube(int pTickIndex)
        {
            if (pTickIndex % _TickBetweenSpawns != 0) return;
            Cube lCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
            lCube.SetColor(_Color);
            timeManager.objectsAffectedByTime.Add(lCube);
            timeManager.onTickFinished += lCube.TickUpdate;
            lCube.onTileDetected += tileManager.TryGetTile;
            lCube.onCubeDeath += gameManager.GameOver;
            lCube.SpawnDirection(direction);
            _CurrentCubeSpawned++;
            if (_CurrentCubeSpawned >= _AmountoOfCubes) StopSpawning();
        }

        private void BeginSpawning()
        {
            if (timeManager == null || _Spawning || _CurrentCubeSpawned >= _AmountoOfCubes) return;

            timeManager.onTickFinished += SpawnCube;
            _Spawning = true;
        }

        private void StopSpawning()
        {
            if (!_Spawning || timeManager == null) return;

            timeManager.onTickFinished -= SpawnCube;
            _Spawning = false;
        }

        private void OnDestroy()
        {
            StopSpawning();
        }
    }
}