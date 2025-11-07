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
        [SerializeField] Cube cubePrefab;

        TimeManager timeManager;

        private Transform self;

        private int _TickBetweenSpawns = 2;

        void Awake()
        {
            tileVariant = TileVariants.Spawner;
            self = transform;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            direction = self.forward;
            timeManager = TimeManager.Instance;
            timeManager.onTickFinished += SpawnCube;
            SpawnCube(0);
        }

        void SpawnCube(int pTickIndex)
        {
            if (pTickIndex % _TickBetweenSpawns != 0) return;
            Cube lCube = Instantiate(cubePrefab, self.position, Quaternion.identity);
            timeManager.objectsAffectedByTime.Add(lCube);
            timeManager.onTickFinished += lCube.TickUpdate;
        }
    }
}
